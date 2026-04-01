using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshRenderer))]
public class PieceHighlighter : MonoBehaviour, IHighlightable
{
    private MeshRenderer Renderer;
    private Color OriginalEmission;

    private void Awake()
    {
        if (!TryGetComponent<MeshRenderer>(out Renderer))
            Debug.Log("PieceHighlighter.cs - Mesh load error");

        OriginalEmission = Renderer.material.GetColor("_EmissiveColor");
    }

    public void HighlightOff()
    {
        Material mat = Renderer.material;
        mat.SetColor("_EmissiveColor", OriginalEmission);
        mat.DisableKeyword("_EMISSION");
    }

    public void HighlightOn()
    {
        Material material = Renderer.material;
        material.SetColor("_EmissiveColor", Color.yellow * 8f);   // 노란색 + 밝기 (8f 조절 가능)
        material.EnableKeyword("_EMISSION");
    }


}
