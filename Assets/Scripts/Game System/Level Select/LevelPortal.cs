using System.Collections;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private const string ClearedColorHex = "#6FE7B7";
    private const string EnableColorHex = "#FFFFFF";
    private const string DisableColorHex = "#FB5B5B";

    public static readonly Color ClearedColor = ParseHexColor(ClearedColorHex, Color.green);
    public static readonly Color EnableColor = ParseHexColor(EnableColorHex, Color.white);
    public static readonly Color DisableColor = ParseHexColor(DisableColorHex, Color.red);

    [SerializeField] private Light PointLight;
    [SerializeField] private Collider Trigger;
    [SerializeField] private SceneId TargetLevel;

    public void Init(bool bEnable, bool bCleared)
    {
        PointLight.color = bEnable ? EnableColor : bCleared ? ClearedColor : DisableColor;
        Trigger.enabled = bEnable || bCleared;
    }

    private static Color ParseHexColor(string hex, Color fallbackColor)
    {
        return ColorUtility.TryParseHtmlString(hex, out Color color) ? color : fallbackColor;
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            if (GameDirector.Instance != null)
            {
                StartCoroutine(LoadLevelRoutine());
            }
        }
    }
    private IEnumerator LoadLevelRoutine()
    {
        yield return new WaitForSeconds(GameDirector.Instance.Iris.FadeOut() + 0.1f);
        GameDirector.Instance.LoadScene(TargetLevel);
    }
}
