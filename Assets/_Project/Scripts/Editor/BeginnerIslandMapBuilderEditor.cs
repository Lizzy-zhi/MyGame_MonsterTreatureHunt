#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MonsterTreasureHunt.Levels
{
    [CustomEditor(typeof(BeginnerIslandMapBuilder))]
    public class BeginnerIslandMapBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Build Full Beginner Island Map"))
            {
                ExecuteForTargets("Build Beginner Island Map", builder => builder.BuildMap());
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Separated Controls", EditorStyles.boldLabel);

            if (GUILayout.Button("Build Ground Only"))
            {
                ExecuteForTargets("Build Beginner Island Ground", builder => builder.BuildGround());
            }

            if (GUILayout.Button("Clear Ground Only"))
            {
                ExecuteForTargets("Clear Beginner Island Ground", builder => builder.ClearGround());
            }

            if (GUILayout.Button("Build Decorations Only"))
            {
                ExecuteForTargets("Build Beginner Island Decorations", builder => builder.BuildDecorations());
            }

            if (GUILayout.Button("Clear Decorations Only"))
            {
                ExecuteForTargets("Clear Beginner Island Decorations", builder => builder.ClearDecorations());
            }

            if (GUILayout.Button("Place Player Spawn Only"))
            {
                ExecuteForTargets("Place Beginner Island Player Spawn", builder => builder.PlacePlayerSpawn());
            }

            if (GUILayout.Button("Place Treasure Only"))
            {
                ExecuteForTargets("Place Beginner Island Treasure", builder => builder.PlaceTreasure());
            }
        }

        private void ExecuteForTargets(string undoName, System.Action<BeginnerIslandMapBuilder> action)
        {
            foreach (BeginnerIslandMapBuilder builder in targets)
            {
                Undo.RegisterFullObjectHierarchyUndo(builder.gameObject, undoName);
                action(builder);
                EditorUtility.SetDirty(builder.gameObject);
            }
        }
    }
}
#endif
