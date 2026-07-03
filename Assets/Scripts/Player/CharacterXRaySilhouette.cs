using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[AddComponentMenu("Trip of Memories/Rendering/Character X-Ray Silhouette")]
public sealed class CharacterXRaySilhouette : MonoBehaviour
{
    private const string MaskShaderName = "TripOfMemories/Character/XRayVisibleMask";
    private const string XRayShaderName = "TripOfMemories/Character/XRaySilhouette";
    private const string MaskSuffix = "_XRayVisibleMask";
    private const string XRaySuffix = "_XRaySilhouette";

    [SerializeField] private Material VisibleMaskMaterial;
    [SerializeField] private Material XRayMaterial;
    [SerializeField] private bool IncludeInactiveRenderers;
    [SerializeField] private bool IncludeMeshRenderers = true;
    [SerializeField] private bool ShowXRay = true;

    private readonly List<OverlayBinding> Overlays = new();
    private Material RuntimeMaskMaterial;
    private Material RuntimeXRayMaterial;

    private void OnEnable()
    {
        Rebuild();
    }

    private void LateUpdate()
    {
        SyncOverlays();
    }

    private void OnDisable()
    {
        SetOverlayRenderersEnabled(false);
    }

    private void OnDestroy()
    {
        ClearOverlays();

        if (RuntimeMaskMaterial)
        {
            Destroy(RuntimeMaskMaterial);
        }

        if (RuntimeXRayMaterial)
        {
            Destroy(RuntimeXRayMaterial);
        }
    }

    [ContextMenu("Rebuild X-Ray Overlays")]
    public void Rebuild()
    {
        ClearOverlays();

        Material maskMaterial = ResolveMaskMaterial();
        Material xRayMaterial = ResolveXRayMaterial();
        if (!maskMaterial || !xRayMaterial)
        {
            Debug.LogWarning($"{nameof(CharacterXRaySilhouette)} could not find required X-ray shaders.", this);
            return;
        }

        foreach (SkinnedMeshRenderer source in GetComponentsInChildren<SkinnedMeshRenderer>(IncludeInactiveRenderers))
        {
            if (IsOverlayRenderer(source))
            {
                continue;
            }

            CreateSkinnedOverlay(source, maskMaterial, xRayMaterial);
        }

        if (IncludeMeshRenderers)
        {
            foreach (MeshRenderer source in GetComponentsInChildren<MeshRenderer>(IncludeInactiveRenderers))
            {
                if (IsOverlayRenderer(source))
                {
                    continue;
                }

                MeshFilter filter = source.GetComponent<MeshFilter>();
                if (!filter || !filter.sharedMesh)
                {
                    continue;
                }

                CreateMeshOverlay(source, filter.sharedMesh, maskMaterial, xRayMaterial);
            }
        }

        SyncOverlays();
    }

    public void SetXRayVisible(bool visible)
    {
        ShowXRay = visible;
        SyncOverlays();
    }

    private Material ResolveMaskMaterial()
    {
        if (VisibleMaskMaterial)
        {
            return VisibleMaskMaterial;
        }

        if (RuntimeMaskMaterial)
        {
            return RuntimeMaskMaterial;
        }

        Shader shader = Shader.Find(MaskShaderName);
        if (!shader)
        {
            return null;
        }

        RuntimeMaskMaterial = new Material(shader)
        {
            name = "Runtime Character XRay Visible Mask",
            hideFlags = HideFlags.DontSave
        };
        return RuntimeMaskMaterial;
    }

    private Material ResolveXRayMaterial()
    {
        if (XRayMaterial)
        {
            return XRayMaterial;
        }

        if (RuntimeXRayMaterial)
        {
            return RuntimeXRayMaterial;
        }

        Shader shader = Shader.Find(XRayShaderName);
        if (!shader)
        {
            return null;
        }

        RuntimeXRayMaterial = new Material(shader)
        {
            name = "Runtime Character XRay Silhouette",
            hideFlags = HideFlags.DontSave
        };
        return RuntimeXRayMaterial;
    }

    private void CreateSkinnedOverlay(SkinnedMeshRenderer source, Material maskMaterial, Material xRayMaterial)
    {
        SkinnedMeshRenderer maskOverlay = CreateSkinnedOverlayRenderer(source, maskMaterial, MaskSuffix);
        SkinnedMeshRenderer xRayOverlay = CreateSkinnedOverlayRenderer(source, xRayMaterial, XRaySuffix);
        Overlays.Add(new OverlayBinding(source, maskOverlay, xRayOverlay));
    }

    private SkinnedMeshRenderer CreateSkinnedOverlayRenderer(SkinnedMeshRenderer source, Material material, string suffix)
    {
        GameObject overlayObject = CreateOverlayObject(source.transform, source.name, suffix);
        SkinnedMeshRenderer overlay = overlayObject.AddComponent<SkinnedMeshRenderer>();

        overlay.sharedMesh = source.sharedMesh;
        overlay.rootBone = source.rootBone;
        overlay.bones = source.bones;
        overlay.localBounds = source.localBounds;
        overlay.updateWhenOffscreen = true;

        CopyRendererSettings(source, overlay);
        AssignRepeatedMaterial(overlay, material, GetMaterialCount(source));
        return overlay;
    }

    private void CreateMeshOverlay(MeshRenderer source, Mesh mesh, Material maskMaterial, Material xRayMaterial)
    {
        MeshRenderer maskOverlay = CreateMeshOverlayRenderer(source, mesh, maskMaterial, MaskSuffix);
        MeshRenderer xRayOverlay = CreateMeshOverlayRenderer(source, mesh, xRayMaterial, XRaySuffix);
        Overlays.Add(new OverlayBinding(source, maskOverlay, xRayOverlay));
    }

    private MeshRenderer CreateMeshOverlayRenderer(MeshRenderer source, Mesh mesh, Material material, string suffix)
    {
        GameObject overlayObject = CreateOverlayObject(source.transform, source.name, suffix);
        MeshFilter overlayFilter = overlayObject.AddComponent<MeshFilter>();
        MeshRenderer overlay = overlayObject.AddComponent<MeshRenderer>();

        overlayFilter.sharedMesh = mesh;
        CopyRendererSettings(source, overlay);
        AssignRepeatedMaterial(overlay, material, GetMaterialCount(source));
        return overlay;
    }

    private static GameObject CreateOverlayObject(Transform source, string sourceName, string suffix)
    {
        GameObject overlayObject = new($"{sourceName}{suffix}");
        overlayObject.hideFlags = HideFlags.DontSave;
        overlayObject.layer = source.gameObject.layer;
        overlayObject.transform.SetParent(source, false);
        return overlayObject;
    }

    private static void CopyRendererSettings(Renderer source, Renderer overlay)
    {
        overlay.shadowCastingMode = ShadowCastingMode.Off;
        overlay.receiveShadows = false;
        overlay.lightProbeUsage = LightProbeUsage.Off;
        overlay.reflectionProbeUsage = ReflectionProbeUsage.Off;
        overlay.allowOcclusionWhenDynamic = false;
        overlay.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        overlay.renderingLayerMask = source.renderingLayerMask;
        overlay.sortingLayerID = source.sortingLayerID;
        overlay.sortingOrder = source.sortingOrder;
    }

    private static void AssignRepeatedMaterial(Renderer renderer, Material material, int count)
    {
        Material[] materials = new Material[Mathf.Max(1, count)];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = material;
        }

        renderer.sharedMaterials = materials;
    }

    private static int GetMaterialCount(Renderer renderer)
    {
        return renderer.sharedMaterials == null ? 1 : renderer.sharedMaterials.Length;
    }

    private void SyncOverlays()
    {
        for (int i = Overlays.Count - 1; i >= 0; i--)
        {
            OverlayBinding binding = Overlays[i];
            if (!binding.Source || !binding.MaskOverlay || !binding.XRayOverlay)
            {
                Overlays.RemoveAt(i);
                continue;
            }

            bool enabled = ShowXRay && binding.Source.enabled && !binding.Source.forceRenderingOff;
            binding.MaskOverlay.enabled = enabled;
            binding.XRayOverlay.enabled = enabled;
            binding.MaskOverlay.forceRenderingOff = binding.Source.forceRenderingOff;
            binding.XRayOverlay.forceRenderingOff = binding.Source.forceRenderingOff;

            if (binding.Source is SkinnedMeshRenderer sourceSkinned &&
                binding.MaskOverlay is SkinnedMeshRenderer maskSkinned &&
                binding.XRayOverlay is SkinnedMeshRenderer xRaySkinned)
            {
                maskSkinned.localBounds = sourceSkinned.localBounds;
                xRaySkinned.localBounds = sourceSkinned.localBounds;
                SyncBlendShapeWeights(sourceSkinned, maskSkinned);
                SyncBlendShapeWeights(sourceSkinned, xRaySkinned);
            }
        }
    }

    private void SetOverlayRenderersEnabled(bool enabled)
    {
        foreach (OverlayBinding binding in Overlays)
        {
            if (binding.MaskOverlay)
            {
                binding.MaskOverlay.enabled = enabled;
            }

            if (binding.XRayOverlay)
            {
                binding.XRayOverlay.enabled = enabled;
            }
        }
    }

    private static void SyncBlendShapeWeights(SkinnedMeshRenderer source, SkinnedMeshRenderer overlay)
    {
        Mesh mesh = source.sharedMesh;
        if (!mesh)
        {
            return;
        }

        int count = mesh.blendShapeCount;
        for (int i = 0; i < count; i++)
        {
            overlay.SetBlendShapeWeight(i, source.GetBlendShapeWeight(i));
        }
    }

    private void ClearOverlays()
    {
        foreach (OverlayBinding binding in Overlays)
        {
            DestroyOverlayObject(binding.MaskOverlay);
            DestroyOverlayObject(binding.XRayOverlay);
        }

        Overlays.Clear();
    }

    private static bool IsOverlayRenderer(Renderer renderer)
    {
        return renderer.name.EndsWith(MaskSuffix) || renderer.name.EndsWith(XRaySuffix);
    }

    private static void DestroyOverlayObject(Renderer overlay)
    {
        if (!overlay)
        {
            return;
        }

        GameObject overlayObject = overlay.gameObject;
        if (Application.isPlaying)
        {
            Destroy(overlayObject);
        }
        else
        {
            DestroyImmediate(overlayObject);
        }
    }

    private readonly struct OverlayBinding
    {
        public readonly Renderer Source;
        public readonly Renderer MaskOverlay;
        public readonly Renderer XRayOverlay;

        public OverlayBinding(Renderer source, Renderer maskOverlay, Renderer xRayOverlay)
        {
            Source = source;
            MaskOverlay = maskOverlay;
            XRayOverlay = xRayOverlay;
        }
    }
}
