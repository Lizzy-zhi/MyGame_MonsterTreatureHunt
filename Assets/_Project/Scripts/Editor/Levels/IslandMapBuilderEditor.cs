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

            if (GUILayout.Button("Build Bee Enemies Only"))
            {
                ExecuteForTargets("Build Selected Map Bee Enemies", builder => builder.BuildBeeEnemies());
            }

            if (GUILayout.Button("Clear Bee Enemies Only"))
            {
                ExecuteForTargets("Clear Selected Map Bee Enemies", builder => builder.ClearBeeEnemies());
            }

            if (GUILayout.Button("Build Fish Enemies Only"))
            {
                ExecuteForTargets("Build Selected Map Fish Enemies", builder => builder.BuildFishEnemies());
            }

            if (GUILayout.Button("Clear Fish Enemies Only"))
            {
                ExecuteForTargets("Clear Selected Map Fish Enemies", builder => builder.ClearFishEnemies());
            }

            if (GUILayout.Button("Build Fire Slime Enemies Only"))
            {
                ExecuteForTargets("Build Selected Map Fire Slime Enemies", builder => builder.BuildFireSlimeEnemies());
            }

            if (GUILayout.Button("Clear Fire Slime Enemies Only"))
            {
                ExecuteForTargets("Clear Selected Map Fire Slime Enemies", builder => builder.ClearFireSlimeEnemies());
            }

            if (GUILayout.Button("Build Ladders Only"))
            {
                ExecuteForTargets("Build Selected Map Ladders", builder => builder.BuildLadders());
            }

            if (GUILayout.Button("Clear Ladders Only"))
            {
                ExecuteForTargets("Clear Selected Map Ladders", builder => builder.ClearLadders());
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
