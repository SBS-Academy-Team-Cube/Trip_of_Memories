#if UNITY_EDITOR // 빌드 제외

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(RoroLevelData))]
public class RoroMapEditor : Editor
{
    private RoroLevelData data;
    private RoroTileType selectedTile = RoroTileType.Wall;

    private LaserStatueDir selectDir = LaserStatueDir.Right;
    private bool editingLaserMode = false;

    public override void OnInspectorGUI()
    {
        data = (RoroLevelData)target;
        data.ReSize();

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("width"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("height"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Tile Palette");
        GUILayout.BeginHorizontal();
        foreach (RoroTileType type in System.Enum.GetValues(typeof(RoroTileType)))
        {
            GUI.backgroundColor = (type == selectedTile) ? Color.green : Color.white;
            if (GUILayout.Button(type.ToString(), GUILayout.Width(80)))
            {
                selectedTile = type;
                editingLaserMode = false;
            }
        }
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("LaserStatue");
        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(editingLaserMode, "LaserDirMode", "Button", GUILayout.Height(30)))
        {
            editingLaserMode = true;
        }
        else
        {
            editingLaserMode = false;
        }

        if (editingLaserMode)
        {
            GUILayout.BeginHorizontal();
            foreach (LaserStatueDir dir in System.Enum.GetValues(typeof(LaserStatueDir)))
            {
                GUI.backgroundColor = (dir == selectDir) ? Color.green : Color.white;
                if (GUILayout.Button(dir.ToString(), GUILayout.Width(70)))
                    selectDir = dir;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space();



        EditorGUILayout.LabelField("Map Grid");
        for (int y = 0; y < data.height; ++y)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < data.width; ++x)
            {
                int index = y * data.width + x;
                Vector2Int pos = new Vector2Int(x, y);

                string buttonText = GetButtonText(pos, index);
                Color bgColor = HasLaserAt(pos) ? Color.darkRed : GetTileColor(data.tiles[index]);

                GUI.backgroundColor = bgColor;

                if (GUILayout.Button(buttonText, GUILayout.Width(40), GUILayout.Height(40)))
                {
                    if (editingLaserMode)
                    {
                        AddOrModifyLaser(pos,index);
                    }
                    else
                    {
                        if (data.tiles[index] == RoroTileType.LaserStatue && selectedTile != RoroTileType.LaserStatue)
                        {
                            RemoveLaserData(pos);
                        }
                        data.tiles[index] = selectedTile;
                    }

                    EditorUtility.SetDirty(data);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUI.backgroundColor = Color.white;
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
            RoroTileType.StartPoint => Color.brown,
            RoroTileType.LaserStatue => Color.darkRed,
            RoroTileType.Player => Color.magenta,
            RoroTileType.Player2 => Color.darkMagenta,
            _ => Color.white
        };
    }

    private string GetButtonText(Vector2Int pos, int index)
    {
        if (editingLaserMode && HasLaserAt(pos))
            return GetLaserDirName(pos);

        return data.tiles[index].ToString();
    }
    private string GetLaserDirName(Vector2Int pos)
    {
        if (data.laserStatues == null) return "";

        foreach (var laser in data.laserStatues)
        {
            if (laser.pos == pos)
                return laser.dir.ToString();
        }
        return "";
    }
    private void AddOrModifyLaser(Vector2Int pos, int index)
    {
        if (data.tiles[index] != RoroTileType.LaserStatue)
            return;


        if (data.laserStatues == null)
            data.laserStatues = new LaserStatueData[0];

        // 이미 있으면 방향만 변경
        for (int i = 0; i < data.laserStatues.Length; i++)
        {
            if (data.laserStatues[i].pos == pos)
            {
                data.laserStatues[i].dir = selectDir;
                return;
            }
        }
        var list = new List<LaserStatueData>(data.laserStatues);
        list.Add(new LaserStatueData
        {
            pos = pos,
            dir = selectDir,
            isActive = true
        });
        data.laserStatues = list.ToArray();
    }
    private void RemoveLaserData(Vector2Int pos)
    {
        if (data.laserStatues == null) return;

        var list = new List<LaserStatueData>();
        foreach (var laser in data.laserStatues)
        {
            if (laser.pos != pos)
                list.Add(laser);
        }
        data.laserStatues = list.ToArray();
    }
    private bool HasLaserAt(Vector2Int pos)
    {
        if (data.laserStatues == null) return false;
        foreach (var laser in data.laserStatues)
            if (laser.pos == pos) return true;
        return false;
    }
}
#endif // 빌드 제외
