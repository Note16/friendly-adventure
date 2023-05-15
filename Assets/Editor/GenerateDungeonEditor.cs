using Assets.Scripts.Dungeon;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class GenerateDungeonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal("box");

        var generator = (DungeonGenerator)target;
        var clickedClear = GUILayout.Button("Clear dungeon");
        if (clickedClear)
            generator.ClearDungeon();

        var clickedGenerate = GUILayout.Button("Generate dungeon");
        if (clickedGenerate)
            generator.GenerateDungeon();

        GUILayout.EndHorizontal();
    }
}