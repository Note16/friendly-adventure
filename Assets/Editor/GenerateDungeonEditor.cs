using Assets.Scripts.Dungeon;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class GenerateDungeonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var generator = (DungeonGenerator)target;
        var clickedGenerate = GUILayout.Button("Generate dungeon");
        if (clickedGenerate)
            generator.GenerateDungeon();
    }
}