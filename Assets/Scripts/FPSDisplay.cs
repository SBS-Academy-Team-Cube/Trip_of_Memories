// using UnityEngine;

// public class FPSDisplay : MonoBehaviour
// {
//     private static FPSDisplay Instance;

//     [Header("Position")]
//     [SerializeField] private Vector2 Position = new Vector2(10.0f, 10.0f);

//     [Header("Style")]
//     [SerializeField] private int FontSize = 24;
//     [SerializeField] private Color TextColor = Color.red;

//     [Header("Update")]
//     [SerializeField] private float RefreshTime = 0.5f;

//     private float Timer;
//     private int FrameCount;
//     private float CurrentFPS;
//     private GUIStyle Style;

//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void Initialize()
//     {
//         GameObject Obj = new GameObject("[FPS Display]");
//         DontDestroyOnLoad(Obj);
//         Obj.AddComponent<FPSDisplay>();
//     }

//     private void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;

//         Style = new GUIStyle
//         {
//             fontSize = FontSize,
//             normal =
//             {
//                 textColor = TextColor
//             }
//         };
//     }

//     private void Update()
//     {
//         FrameCount++;
//         Timer += Time.unscaledDeltaTime;

//         if (Timer >= RefreshTime)
//         {
//             CurrentFPS = FrameCount / Timer;

//             FrameCount = 0;
//             Timer = 0.0f;
//         }
//     }

//     private void OnGUI()
//     {
//         float Ms = 1000.0f / Mathf.Max(CurrentFPS, 0.0001f);

//         string Text =
//             $"FPS : {CurrentFPS:F1}\n" +
//             $"MS  : {Ms:F1}";

//         GUI.Label(
//             new Rect(Position.x, Position.y, 200.0f, 60.0f),
//             Text,
//             Style);
//     }
// }