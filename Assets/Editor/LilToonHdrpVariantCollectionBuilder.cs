using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
internal static class LilToonHdrpVariantCollectionBuilder
{
    private const string CollectionDirectory = "Assets/ProjectAsset/ShaderVariants";
    private const string CollectionPath = CollectionDirectory + "/LilToonHdrpRuntimeVariants.shadervariants";
    private const string MenuPath = "Tools/Build/Refresh lilToon HDRP Runtime Variants";

    private static readonly string[][] HdrpShadowKeywordProfiles =
    {
        new[] { "PUNCTUAL_SHADOW_MEDIUM", "DIRECTIONAL_SHADOW_MEDIUM", "AREA_SHADOW_MEDIUM" },
        new[] { "PUNCTUAL_SHADOW_MEDIUM", "DIRECTIONAL_SHADOW_HIGH", "AREA_SHADOW_MEDIUM" },
    };
    private static readonly HashSet<string> TerrainGrassInstancedPasses = new(StringComparer.Ordinal)
    {
        "ShadowCaster",
        "MotionVectors",
        "DepthOnly",
        "GBuffer",
    };
    private static readonly VariantDefinition[] RequiredVariants = CreateRequiredVariants();

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

    internal static void EnsureCollection()
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
                    definition.PassType,
                    definition.Keywords));
            }
            catch (ArgumentException exception)
            {
                Debug.LogError("[Build] Failed to preserve lilToon variant for " + definition.ShaderName + ": " + exception.Message);
            }
        }

        EditorUtility.SetDirty(collection);
        AddToPreloadedShaders(collection);
        AddToAlwaysIncludedShaders(
            "lilToon",
            "Hidden/lilToonOutline",
            "Hidden/lilToonTransparentOutline");
        AssetDatabase.SaveAssets();
    }

    internal static bool MustPreserveVariant(
        Shader shader,
        ShaderSnippetData snippet,
        ShaderCompilerData compilerData)
    {
        // These are the real UsePass owners. Their build inputs are small, and HDRP may request
        // runtime keyword combinations that are no longer visible through the wrapper shaders.
        if (shader.name == "Hidden/ltspass_opaque" ||
            shader.name == "Hidden/ltspass_transparent" ||
            shader.name == "lilToon" ||
            shader.name == "Hidden/lilToonOutline" ||
            shader.name == "Hidden/lilToonTransparentOutline")
        {
            return true;
        }

        var keywords = compilerData.shaderKeywordSet
            .GetShaderKeywords()
            .Select(keyword => keyword.name)
            .ToHashSet(StringComparer.Ordinal);

        // TerrainGrass is used by Stage 2 foliage. Preserve only the deferred raster passes
        // drawn by alpha-cutout instanced foliage, not disabled forward or ray-tracing passes.
        if (shader.name == "Shader Graphs/TerrainGrass" &&
            TerrainGrassInstancedPasses.Contains(snippet.passName) &&
            keywords.Contains("INSTANCING_ON") &&
            keywords.Contains("_ALPHATEST_ON"))
        {
            return true;
        }

        return RequiredVariants.Any(definition =>
            definition.Matches(shader.name, snippet.passType, keywords));
    }

    private static VariantDefinition[] CreateRequiredVariants()
    {
        var variants = new List<VariantDefinition>();

        // Runtime materials use wrappers, while Unity reports missing UsePass variants by the implementation shader.
        AddForwardVariants(variants, "lilToon", PassType.ScriptableRenderPipeline, "USE_FPTL_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/lilToonOutline", PassType.ScriptableRenderPipeline, "USE_FPTL_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/lilToonOutline", PassType.ScriptableRenderPipelineDefaultUnlit, "USE_FPTL_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/ltspass_opaque", PassType.ScriptableRenderPipeline, "USE_FPTL_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/ltspass_opaque", PassType.ScriptableRenderPipelineDefaultUnlit, "USE_FPTL_LIGHTLIST");

        AddForwardVariants(variants, "Hidden/lilToonTransparentOutline", PassType.ScriptableRenderPipeline, "USE_CLUSTERED_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/lilToonTransparentOutline", PassType.ScriptableRenderPipelineDefaultUnlit, "USE_CLUSTERED_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/ltspass_transparent", PassType.ScriptableRenderPipeline, "USE_CLUSTERED_LIGHTLIST");
        AddForwardVariants(variants, "Hidden/ltspass_transparent", PassType.ScriptableRenderPipelineDefaultUnlit, "USE_CLUSTERED_LIGHTLIST");

        // HDRP 17 uses a combined decal/rendering-layer keyword in its MotionVectors pass.
        variants.Add(new VariantDefinition("lilToon", PassType.MotionVectors, "WRITE_DECAL_BUFFER_AND_RENDERING_LAYER"));
        variants.Add(new VariantDefinition("Hidden/lilToonOutline", PassType.MotionVectors, "WRITE_DECAL_BUFFER_AND_RENDERING_LAYER"));
        variants.Add(new VariantDefinition("Hidden/lilToonTransparentOutline", PassType.MotionVectors, "WRITE_DECAL_BUFFER_AND_RENDERING_LAYER"));
        variants.Add(new VariantDefinition("Hidden/ltspass_opaque", PassType.MotionVectors, "WRITE_DECAL_BUFFER_AND_RENDERING_LAYER"));
        variants.Add(new VariantDefinition("Hidden/ltspass_transparent", PassType.MotionVectors, "WRITE_DECAL_BUFFER_AND_RENDERING_LAYER"));

        return variants.ToArray();
    }

    private static void AddForwardVariants(
        ICollection<VariantDefinition> variants,
        string shaderName,
        PassType passType,
        string lightListKeyword)
    {
        foreach (var probeVolumeKeyword in new[] { "PROBE_VOLUMES_L1", "PROBE_VOLUMES_L2" })
        {
            foreach (var shadowKeywords in HdrpShadowKeywordProfiles)
            {
                AddForwardVariant(variants, shaderName, passType, probeVolumeKeyword, lightListKeyword, shadowKeywords);
                AddForwardVariant(variants, shaderName, passType, probeVolumeKeyword, lightListKeyword, shadowKeywords, "SCREEN_SPACE_SHADOWS_OFF");
                AddForwardVariant(variants, shaderName, passType, probeVolumeKeyword, lightListKeyword, shadowKeywords, "SCREEN_SPACE_SHADOWS_ON");
            }
        }
    }

    private static void AddForwardVariant(
        ICollection<VariantDefinition> variants,
        string shaderName,
        PassType passType,
        string probeVolumeKeyword,
        string lightListKeyword,
        IEnumerable<string> shadowKeywords,
        string screenSpaceShadowsKeyword = null)
    {
        var keywords = new List<string> { probeVolumeKeyword, lightListKeyword };
        if (screenSpaceShadowsKeyword != null)
        {
            keywords.Add(screenSpaceShadowsKeyword);
        }

        keywords.AddRange(shadowKeywords);
        variants.Add(new VariantDefinition(shaderName, passType, keywords.ToArray()));
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
        var serializedSettings = LoadGraphicsSettings();
        if (serializedSettings == null)
        {
            return;
        }

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

    private static void AddToAlwaysIncludedShaders(params string[] shaderNames)
    {
        var serializedSettings = LoadGraphicsSettings();
        if (serializedSettings == null)
        {
            return;
        }

        var alwaysIncludedShaders = serializedSettings.FindProperty("m_AlwaysIncludedShaders");

        foreach (var shaderName in shaderNames)
        {
            var shader = Shader.Find(shaderName);
            if (shader == null)
            {
                Debug.LogError("[Build] Cannot always include lilToon shader because it was not found: " + shaderName);
                continue;
            }

            var isIncluded = false;
            for (var index = 0; index < alwaysIncludedShaders.arraySize; index++)
            {
                if (alwaysIncludedShaders.GetArrayElementAtIndex(index).objectReferenceValue == shader)
                {
                    isIncluded = true;
                    break;
                }
            }

            if (!isIncluded)
            {
                var elementIndex = alwaysIncludedShaders.arraySize;
                alwaysIncludedShaders.InsertArrayElementAtIndex(elementIndex);
                alwaysIncludedShaders.GetArrayElementAtIndex(elementIndex).objectReferenceValue = shader;
            }
        }

        serializedSettings.ApplyModifiedPropertiesWithoutUndo();
    }

    private static SerializedObject LoadGraphicsSettings()
    {
        var settingsObjects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset");
        if (settingsObjects.Length == 0)
        {
            Debug.LogError("[Build] Unable to load GraphicsSettings for required lilToon shaders.");
            return null;
        }

        return new SerializedObject(settingsObjects[0]);
    }

    private readonly struct VariantDefinition
    {
        public VariantDefinition(string shaderName, PassType passType, params string[] keywords)
        {
            ShaderName = shaderName;
            PassType = passType;
            Keywords = keywords;
        }

        public string ShaderName { get; }

        public PassType PassType { get; }

        public string[] Keywords { get; }

        public bool Matches(string shaderName, PassType passType, HashSet<string> keywords)
        {
            return ShaderName == shaderName &&
                   PassType == passType &&
                   keywords.SetEquals(Keywords);
        }
    }
}

internal sealed class LilToonHdrpVariantCollectionBuildPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder => -1000;

    public void OnPreprocessBuild(BuildReport report)
    {
        LilToonHdrpVariantCollectionBuilder.EnsureCollection();
    }
}

public sealed class LilToonHdrpRequiredVariantStripper : IShaderVariantStripper
{
    public bool active => true;

    public bool CanRemoveVariant(Shader shader, ShaderSnippetData shaderVariant, ShaderCompilerData shaderCompilerData)
    {
        return !LilToonHdrpVariantCollectionBuilder.MustPreserveVariant(
            shader,
            shaderVariant,
            shaderCompilerData);
    }
}
