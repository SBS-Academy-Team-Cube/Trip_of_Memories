using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoroLevelData))]
public class RoroMapEditor : Editor
{
    private RoroLevelData data;
    private RoroTileType selectedTile = RoroTileType.Wall;

    public override void OnInspectorGUI()
    {
        data = (RoroLevelData)target;
        data.ReSize();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("width"));
        EditorGUILayout.PropertyField (serializedObject.FindProperty("height"));

        EditorGUILayout.LabelField("Tile Palette");
        GUILayout.BeginHorizontal();
        foreach(RoroTileType type in System.Enum.GetValues(typeof(RoroTileType)))
        {
            GUI.backgroundColor = (type == selectedTile) ? Color.green : Color.white;
            if (GUILayout.Button(type.ToString(), GUILayout.Width(80)))
                selectedTile = type;
        }
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;

        EditorGUILayout.LabelField("Map Grid");
        for(int y = 0; y < data.height; ++y)
        {
            GUILayout.BeginHorizontal();
            for(int x = 0; x < data.width; ++x)
            {
                int index = y * data.width + x;
                RoroTileType current = data.tiles[index];

                GUI.contentColor = GetTileColor(current);
                if (GUILayout.Button(current.ToString(), GUILayout.Width(30), GUILayout.Height(30)))
                {
                    data.tiles[index] = selectedTile;
                    EditorUtility.SetDirty(data);
                }
            }
            GUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();

    }
    
    private Color GetTileColor(RoroTileType type)
    {
        return type switch
        {
            RoroTileType.Empty => Color.white,
            RoroTileType.Wall => Color.gray,
            RoroTileType.PushBox => Color.orange,
            RoroTileType.Enemy => Color.red,
            RoroTileType.Key => Color.green,
            RoroTileType.Ice => Color.lightBlue,
            RoroTileType.Water => Color.blue,
            RoroTileType.Exit => Color.brown,
            _ => Color.white
        };
    }


}
