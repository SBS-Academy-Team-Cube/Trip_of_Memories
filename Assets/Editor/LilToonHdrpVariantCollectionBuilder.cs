using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
internal static class LilToonHdrpVariantCollectionBuilder
{
    private const string CollectionDirectory = "Assets/ProjectAsset/ShaderVariants";
    private const string CollectionPath = CollectionDirectory + "/LilToonHdrpRuntimeVariants.shadervariants";
    private const string MenuPath = "Tools/Build/Refresh lilToon HDRP Runtime Variants";

    private static readonly VariantDefinition[] RequiredVariants =
    {
        new VariantDefinition("lilToon", "PROBE_VOLUMES_L2", "SCREEN_SPACE_SHADOWS_ON", "USE_FPTL_LIGHTLIST"),
        new VariantDefinition("Hidden/lilToonOutline", "PROBE_VOLUMES_L2", "SCREEN_SPACE_SHADOWS_ON", "USE_FPTL_LIGHTLIST"),
        new VariantDefinition("Hidden/lilToonOutline", "PROBE_VOLUMES_L2", "USE_FPTL_LIGHTLIST"),
        new VariantDefinition("Hidden/lilToonTransparentOutline", "PROBE_VOLUMES_L2", "SCREEN_SPACE_SHADOWS_ON", "USE_CLUSTERED_LIGHTLIST"),
        new VariantDefinition("Hidden/lilToonTransparentOutline", "PROBE_VOLUMES_L2", "USE_CLUSTERED_LIGHTLIST"),
    };

    static LilToonHdrpVariantCollectionBuilder()
    {
        EditorApplication.delayCall += EnsureCollection;
    }

    [MenuItem(MenuPath)]
    private static void RefreshCollection()
    {
        EnsureCollection();
        Debug.Log("[Build] Refreshed required lilToon HDRP shader variants.");
    }

    private static void EnsureCollection()
    {
        EnsureFolder();

        var collection = AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(CollectionPath);
        if (collection == null)
        {
            collection = new ShaderVariantCollection();
            AssetDatabase.CreateAsset(collection, CollectionPath);
        }

        collection.Clear();

        foreach (var definition in RequiredVariants)
        {
            var shader = Shader.Find(definition.ShaderName);
            if (shader == null)
            {
                Debug.LogError("[Build] Cannot preserve lilToon variant because shader was not found: " + definition.ShaderName);
                continue;
            }

            try
            {
                collection.Add(new ShaderVariantCollection.ShaderVariant(
                    shader,
                    PassType.ScriptableRenderPipeline,
                    definition.Keywords));
            }
            catch (ArgumentException exception)
            {
                Debug.LogError("[Build] Failed to preserve lilToon variant for " + definition.ShaderName + ": " + exception.Message);
            }
        }

        EditorUtility.SetDirty(collection);
        AddToPreloadedShaders(collection);
        AssetDatabase.SaveAssets();
    }

    private static void EnsureFolder()
    {
        const string parent = "Assets/ProjectAsset";

        if (!AssetDatabase.IsValidFolder(CollectionDirectory))
        {
            AssetDatabase.CreateFolder(parent, "ShaderVariants");
        }
    }

    private static void AddToPreloadedShaders(ShaderVariantCollection collection)
    {
        var settingsObjects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset");
        if (settingsObjects.Length == 0)
        {
            Debug.LogError("[Build] Unable to load GraphicsSettings for lilToon variant preloading.");
            return;
        }

        var serializedSettings = new SerializedObject(settingsObjects[0]);
        var preloadedShaders = serializedSettings.FindProperty("m_PreloadedShaders");

        for (var index = 0; index < preloadedShaders.arraySize; index++)
        {
            if (preloadedShaders.GetArrayElementAtIndex(index).objectReferenceValue == collection)
            {
                return;
            }
        }

        var elementIndex = preloadedShaders.arraySize;
        preloadedShaders.InsertArrayElementAtIndex(elementIndex);
        preloadedShaders.GetArrayElementAtIndex(elementIndex).objectReferenceValue = collection;
        serializedSettings.ApplyModifiedPropertiesWithoutUndo();
    }

    private readonly struct VariantDefinition
    {
        public VariantDefinition(string shaderName, params string[] keywords)
        {
            ShaderName = shaderName;
            Keywords = keywords;
        }

        public string ShaderName { get; }

        public string[] Keywords { get; }
    }
}
