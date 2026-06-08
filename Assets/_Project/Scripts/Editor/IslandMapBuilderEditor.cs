#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MonsterTreasureHunt.Levels
{
    [CustomEditor(typeof(IslandMapBuilder))]
    public class IslandMapBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Build Selected Map"))
            {
                ExecuteForTargets("Build Selected Map", builder => builder.BuildMap());
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Separated Controls", EditorStyles.boldLabel);

            if (GUILayout.Button("Build Ground Only"))
            {
                ExecuteForTargets("Build Selected Map Ground", builder => builder.BuildGround());
            }

            if (GUILayout.Button("Clear Ground Only"))
            {
                ExecuteForTargets("Clear Map Ground", builder => builder.ClearGround());
            }

            if (GUILayout.Button("Build Decorations Only"))
            {
                ExecuteForTargets("Build Selected Map Decorations", builder => builder.BuildDecorations());
            }

            if (GUILayout.Button("Clear Decorations Only"))
            {
                ExecuteForTargets("Clear Map Decorations", builder => builder.ClearDecorations());
            }

            if (GUILayout.Button("Place Player Spawn Only"))
            {
                ExecuteForTargets("Place Selected Map Player Spawn", builder => builder.PlacePlayerSpawn());
            }

            if (GUILayout.Button("Place Treasure Only"))
            {
                ExecuteForTargets("Place Selected Map Treasure", builder => builder.PlaceTreasure());
            }
        }

        private void ExecuteForTargets(string undoName, System.Action<IslandMapBuilder> action)
        {
            foreach (IslandMapBuilder builder in targets)
            {
                Undo.RegisterFullObjectHierarchyUndo(builder.gameObject, undoName);
                action(builder);
                EditorUtility.SetDirty(builder.gameObject);
            }
        }
    }
}
#endif
