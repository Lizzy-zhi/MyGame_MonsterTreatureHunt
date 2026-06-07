using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using MonsterTreasureHunt.Gameplay;

namespace MonsterTreasureHunt.Levels
{
    [DisallowMultipleComponent]
    public class BeginnerIslandMapBuilder : MonoBehaviour
    {
        public enum MapTheme
        {
            BeginnerIsland = 0,
            FoggyForest = 1,
            VolcanoCave = 2
        }

        [Serializable]
        private struct PlatformSegment
        {
            public int xMin;
            public int xMax;
            public int surfaceY;
            public int depth;
        }

        [Header("Tilemaps")]
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap decorationTilemap;

        [Header("Grass Tiles")]
        [SerializeField] private TileBase topLeft;
        [SerializeField] private TileBase topCenter;
        [SerializeField] private TileBase topRight;
        [SerializeField] private TileBase center;
        [SerializeField] private TileBase bottom;
        [SerializeField] private TileBase bottomLeft;
        [SerializeField] private TileBase bottomRight;
        [SerializeField] private TileBase leftEdge;
        [SerializeField] private TileBase rightEdge;

        [Header("Decorations")]
        [SerializeField] private TileBase signTile;
        [SerializeField] private TileBase bushTile;
        [SerializeField] private TileBase rockTile;
        [SerializeField] private TileBase bridgeTile;
        [SerializeField] private TileBase waterTile;
        [SerializeField] private TileBase waterTopTile;
        [SerializeField] private TileBase fenceTile;
        [SerializeField] private TileBase mushroomRedTile;
        [SerializeField] private TileBase mushroomBrownTile;

        [Header("Gameplay")]
        [SerializeField] private Transform playerSpawn;
        [SerializeField] private Transform treasure;
        [SerializeField] private float spawnHeightOffset = 0.5f;

        [Header("Health Pickups")]
        [SerializeField] private Sprite healthPickupSprite;
        [SerializeField] private int healthPickupSortingOrder = 2;
        [SerializeField] private float healthPickupHeightOffset = 0.75f;
        [SerializeField] private Vector2 healthPickupColliderSize = new Vector2(1.2f, 1.2f);
        [SerializeField] private float healthPickupVisualScale = 2f;
        [SerializeField] private Transform healthPickupParent;

        [Header("Key Pickup")]
        [SerializeField] private Sprite keyPickupSprite;
        [SerializeField] private int keyPickupSortingOrder = 3;
        [SerializeField] private float keyPickupHeightOffset = 0.85f;
        [SerializeField] private float keyPickupVisualScale = 1.5f;
        [SerializeField] private Vector2 keyPickupColliderSize = new Vector2(1.1f, 1.1f);
        [SerializeField] private Transform keyPickupParent;

        [Header("Build")]
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private bool buildGroundOnStart = true;
        [SerializeField] private bool buildDecorationsOnStart = false;
        [SerializeField] private bool placePlayerOnStart = true;
        [SerializeField] private bool placeTreasureOnStart = true;
        [SerializeField] private MapTheme selectedMap = MapTheme.BeginnerIsland;

        private const int PlayerSpawnCellX = -26;
        private const int PlayerSpawnSurfaceY = -5;
        private const int TreasureCellX = 98;
        private const int TreasureSurfaceY = -3;
        private const int FoggyForestTreasureCellX = 74;
        private const int FoggyForestTreasureSurfaceY = -3;

        private static readonly PlatformSegment[] DefaultLayout =
        {
            // Main running ground
            new() { xMin = -28, xMax = -16, surfaceY = -5, depth = 6 },
            new() { xMin = -15, xMax = -2, surfaceY = -5, depth = 6 },
            new() { xMin = -1, xMax = 12, surfaceY = -5, depth = 6 },
            new() { xMin = 13, xMax = 24, surfaceY = -4, depth = 5 },
            new() { xMin = 25, xMax = 37, surfaceY = -4, depth = 5 },
            new() { xMin = 38, xMax = 50, surfaceY = -4, depth = 5 },
            new() { xMin = 51, xMax = 63, surfaceY = -3, depth = 5 },
            new() { xMin = 64, xMax = 76, surfaceY = -3, depth = 5 },
            new() { xMin = 77, xMax = 89, surfaceY = -3, depth = 5 },
            new() { xMin = 90, xMax = 104, surfaceY = -3, depth = 5 },

            // Floating platform groups (runner style)
            new() { xMin = -10, xMax = -6, surfaceY = -4, depth = 1 },
            new() { xMin = -2, xMax = 2, surfaceY = -4, depth = 1 },
            new() { xMin = 6, xMax = 10, surfaceY = -4, depth = 1 },
            new() { xMin = 15, xMax = 20, surfaceY = -3, depth = 1 },
            new() { xMin = 24, xMax = 28, surfaceY = -3, depth = 1 },
            new() { xMin = 33, xMax = 38, surfaceY = -3, depth = 1 },
            new() { xMin = 42, xMax = 47, surfaceY = -3, depth = 1 },
            new() { xMin = 52, xMax = 56, surfaceY = -2, depth = 1 },
            new() { xMin = 60, xMax = 64, surfaceY = -2, depth = 1 },
            new() { xMin = 69, xMax = 74, surfaceY = -2, depth = 1 },
            new() { xMin = 79, xMax = 84, surfaceY = -2, depth = 1 },
            new() { xMin = 88, xMax = 93, surfaceY = -2, depth = 1 },
        };

        private static readonly PlatformSegment[] FoggyForestLayout =
        {
            // Main forest floor: compact second-map route with small readable gaps and gentle rises.
            new() { xMin = -28, xMax = -16, surfaceY = -5, depth = 6 },
            new() { xMin = -15, xMax = -2, surfaceY = -5, depth = 6 },
            new() { xMin = -1, xMax = 10, surfaceY = -5, depth = 6 },
            new() { xMin = 12, xMax = 23, surfaceY = -4, depth = 5 },
            new() { xMin = 25, xMax = 36, surfaceY = -4, depth = 5 },
            new() { xMin = 38, xMax = 49, surfaceY = -4, depth = 5 },
            new() { xMin = 51, xMax = 62, surfaceY = -3, depth = 5 },
            new() { xMin = 64, xMax = 78, surfaceY = -3, depth = 5 },

            // Upper canopy route: short optional exploration path with beginner-friendly jumps.
            new() { xMin = -12, xMax = -8, surfaceY = -3, depth = 1 },
            new() { xMin = -4, xMax = 1, surfaceY = -3, depth = 1 },
            new() { xMin = 6, xMax = 11, surfaceY = -3, depth = 1 },
            new() { xMin = 16, xMax = 22, surfaceY = -2, depth = 1 },
            new() { xMin = 28, xMax = 34, surfaceY = -2, depth = 1 },
            new() { xMin = 42, xMax = 48, surfaceY = -1, depth = 1 },
            new() { xMin = 56, xMax = 62, surfaceY = -1, depth = 1 },
        };

        private static readonly PlatformSegment[] VolcanoCaveLayout =
        {
            new() { xMin = -28, xMax = -17, surfaceY = -5, depth = 6 },
            new() { xMin = -15, xMax = -4, surfaceY = -5, depth = 6 },
            new() { xMin = -2, xMax = 8, surfaceY = -5, depth = 6 },
            new() { xMin = 10, xMax = 20, surfaceY = -4, depth = 5 },
            new() { xMin = 22, xMax = 32, surfaceY = -4, depth = 5 },
            new() { xMin = 34, xMax = 45, surfaceY = -3, depth = 5 },
            new() { xMin = 47, xMax = 58, surfaceY = -3, depth = 5 },
            new() { xMin = 60, xMax = 72, surfaceY = -3, depth = 5 },
            new() { xMin = 74, xMax = 88, surfaceY = -3, depth = 5 },
            new() { xMin = 90, xMax = 106, surfaceY = -3, depth = 5 },

            new() { xMin = -10, xMax = -6, surfaceY = -4, depth = 1 },
            new() { xMin = -1, xMax = 3, surfaceY = -4, depth = 1 },
            new() { xMin = 7, xMax = 11, surfaceY = -3, depth = 1 },
            new() { xMin = 16, xMax = 21, surfaceY = -3, depth = 1 },
            new() { xMin = 27, xMax = 31, surfaceY = -2, depth = 1 },
            new() { xMin = 39, xMax = 43, surfaceY = -2, depth = 1 },
            new() { xMin = 52, xMax = 56, surfaceY = -2, depth = 1 },
            new() { xMin = 66, xMax = 70, surfaceY = -2, depth = 1 },
            new() { xMin = 81, xMax = 85, surfaceY = -2, depth = 1 },
            new() { xMin = 97, xMax = 101, surfaceY = -2, depth = 1 },
        };

        private void Start()
        {
            if (!buildOnStart) return;

            if (buildGroundOnStart)
            {
                BuildGround();
            }

            if (buildDecorationsOnStart || SelectedMapUsesDecorations())
            {
                BuildDecorations();
            }
            else
            {
                ClearDecorations();
            }

            if (placePlayerOnStart)
            {
                PlacePlayerSpawn();
            }

            if (placeTreasureOnStart)
            {
                PlaceTreasure();
            }
        }

        [ContextMenu("Build Selected Map")]
        public void BuildMap()
        {
            BuildGround();
            BuildDecorations();
            PlaceGameplayObjects();
        }

        [ContextMenu("Build Selected Map Ground")]
        public void BuildGround()
        {
            EnsureTilemapsAssigned();

            if (groundTilemap == null)
            {
                Debug.LogError("[BeginnerIslandMapBuilder] Ground tilemap is not assigned.");
                return;
            }

            groundTilemap.ClearAllTiles();

            foreach (PlatformSegment segment in GetLayoutForSelectedMap())
            {
                BuildPlatform(segment);
            }

            RefreshColliders();
        }

        [ContextMenu("Clear Map Ground")]
        public void ClearGround()
        {
            EnsureTilemapsAssigned();
            if (groundTilemap == null) return;

            groundTilemap.ClearAllTiles();
            RefreshColliders();
        }

        [ContextMenu("Build Selected Map Decorations")]
        public void BuildDecorations()
        {
            EnsureTilemapsAssigned();
            ClearDecorations();

            if (selectedMap == MapTheme.FoggyForest)
            {
                PlaceFoggyForestDecorations();
            }
        }

        [ContextMenu("Clear Map Decorations")]
        public void ClearDecorations()
        {
            EnsureTilemapsAssigned();
            if (decorationTilemap == null) return;

            decorationTilemap.ClearAllTiles();
        }

        private void BuildPlatform(PlatformSegment segment)
        {
            int width = segment.xMax - segment.xMin + 1;

            for (int x = segment.xMin; x <= segment.xMax; x++)
            {
                bool isLeft = x == segment.xMin;
                bool isRight = x == segment.xMax;
                bool singleColumn = width == 1;

                for (int layer = 0; layer < segment.depth; layer++)
                {
                    int y = segment.surfaceY - layer;
                    bool isTop = layer == 0;
                    bool isBottom = layer == segment.depth - 1;

                    TileBase tile = PickTile(isTop, isBottom, isLeft, isRight, singleColumn);
                    if (tile == null) continue;

                    groundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        private TileBase PickTile(bool isTop, bool isBottom, bool isLeft, bool isRight, bool singleColumn)
        {
            if (isTop)
            {
                if (singleColumn) return topCenter;
                if (isLeft) return topLeft;
                if (isRight) return topRight;
                return topCenter;
            }

            if (isBottom)
            {
                if (singleColumn) return bottom;
                if (isLeft) return bottomLeft;
                if (isRight) return bottomRight;
                return bottom;
            }

            if (singleColumn) return center;
            if (isLeft) return leftEdge;
            if (isRight) return rightEdge;
            return center;
        }

        private void PlaceDecorations()
        {
            if (decorationTilemap == null) return;

            SetSurfaceDecoration(-27, -5, signTile);
            SetSurfaceDecoration(-20, -5, bushTile);
            SetSurfaceDecoration(-8, -5, rockTile);
            SetSurfaceDecoration(4, -5, bushTile);
            SetSurfaceDecoration(18, -4, bushTile);
            SetSurfaceDecoration(31, -4, rockTile);
            SetSurfaceDecoration(45, -4, bushTile);
            SetSurfaceDecoration(58, -3, rockTile);
            SetSurfaceDecoration(73, -3, signTile);

            BuildWaterStrip(-36, -29, -7);
            BuildWaterStrip(77, 86, -5);
        }

        private void PlaceFoggyForestDecorations()
        {
            if (decorationTilemap == null) return;

            SetSurfaceDecoration(-27, -5, signTile);
            SetSurfaceDecoration(-21, -5, bushTile);
            SetSurfaceDecoration(-13, -5, rockTile);
            SetSurfaceDecoration(-5, -5, mushroomBrownTile);
            SetSurfaceDecoration(4, -5, mushroomRedTile);
            SetSurfaceDecoration(14, -4, bushTile);
            SetSurfaceDecoration(22, -4, rockTile);
            SetSurfaceDecoration(27, -4, fenceTile);
            SetSurfaceDecoration(28, -4, fenceTile);
            SetSurfaceDecoration(29, -4, fenceTile);
            SetSurfaceDecoration(31, -4, mushroomBrownTile);
            SetSurfaceDecoration(40, -4, bushTile);
            SetSurfaceDecoration(47, -4, mushroomRedTile);
            SetSurfaceDecoration(56, -3, rockTile);
            SetSurfaceDecoration(66, -3, bushTile);
            SetSurfaceDecoration(70, -3, fenceTile);
            SetSurfaceDecoration(71, -3, fenceTile);
            SetSurfaceDecoration(78, -3, mushroomRedTile);
        }

        private void BuildWaterStrip(int xMin, int xMax, int topY)
        {
            BuildDecorationLine(xMin, xMax, topY, waterTopTile);
            BuildDecorationLine(xMin, xMax, topY - 1, waterTile);
        }

        private void BuildDecorationLine(int xMin, int xMax, int y, TileBase tile)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                SetDecoration(x, y, tile);
            }
        }

        private void SetSurfaceDecoration(int x, int surfaceY, TileBase tile)
        {
            SetDecoration(x, surfaceY + 1, tile);
        }

        private void SetDecoration(int x, int y, TileBase tile)
        {
            if (tile == null) return;

            decorationTilemap.SetTile(new Vector3Int(x, y, 0), tile);
        }

        [ContextMenu("Place Selected Map Gameplay Objects")]
        public void PlaceGameplayObjects()
        {
            PlacePlayerSpawn();
            PlaceTreasure();
            BuildHealthPickups();
            BuildKeyPickup();
        }

        public void SelectMap(MapTheme map)
        {
            selectedMap = map;
        }

        [ContextMenu("Place Selected Map Player Spawn")]
        public void PlacePlayerSpawn()
        {
            GetPlacementForSelectedMap(out int spawnCellX, out int spawnSurfaceY, out _, out _);
            if (!TryGetSurfaceWorldPosition(spawnCellX, spawnSurfaceY, out Vector3 spawnWorld)) return;

            if (playerSpawn != null)
            {
                playerSpawn.position = spawnWorld;
            }

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerObject.transform.position = spawnWorld;
            }
        }

        [ContextMenu("Place Selected Map Treasure")]
        public void PlaceTreasure()
        {
            GetPlacementForSelectedMap(out _, out _, out int treasureCellX, out int treasureSurfaceY);
            if (!TryGetSurfaceWorldPosition(treasureCellX, treasureSurfaceY, out Vector3 treasureWorld)) return;

            if (treasure != null)
            {
                treasure.position = treasureWorld;
            }
        }

        [ContextMenu("Build Health Pickups")]
        public void BuildHealthPickups()
        {
            ClearHealthPickups();

            if (healthPickupSprite == null) return;

            Vector2Int[] placements = GetHealthPickupPlacementsForSelectedMap();
            for (int i = 0; i < placements.Length; i++)
            {
                Vector2Int placement = placements[i];
                if (!TryGetPickupWorldPosition(placement.x, placement.y, out Vector3 worldPosition)) continue;

                CreateHealthPickup(i + 1, worldPosition);
            }
        }

        [ContextMenu("Clear Health Pickups")]
        public void ClearHealthPickups()
        {
            Transform parent = GetHealthPickupParent();
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child != null && child.name.StartsWith("HealthPickup_", StringComparison.Ordinal))
                {
                    if (Application.isPlaying)
                    {
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        private void CreateHealthPickup(int index, Vector3 worldPosition)
        {
            GameObject pickupObject = new GameObject($"HealthPickup_{index:00}");
            pickupObject.transform.SetParent(GetHealthPickupParent(), true);
            pickupObject.transform.position = worldPosition;

            SpriteRenderer renderer = pickupObject.AddComponent<SpriteRenderer>();
            renderer.sprite = healthPickupSprite;
            renderer.sortingOrder = healthPickupSortingOrder;
            pickupObject.transform.localScale = Vector3.one * healthPickupVisualScale;

            BoxCollider2D collider = pickupObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = healthPickupColliderSize / Mathf.Max(0.01f, healthPickupVisualScale);

            pickupObject.AddComponent<HealthPickup>();
        }

        [ContextMenu("Build Key Pickup")]
        public void BuildKeyPickup()
        {
            ClearKeyPickups();
            if (keyPickupSprite == null) return;

            Vector2Int placement = GetKeyPickupPlacementForSelectedMap();
            if (!TryGetItemWorldPosition(placement.x, placement.y, keyPickupHeightOffset, out Vector3 worldPosition)) return;

            GameObject pickupObject = new GameObject("KeyPickup_Yellow");
            pickupObject.transform.SetParent(GetKeyPickupParent(), true);
            pickupObject.transform.position = worldPosition;
            pickupObject.transform.localScale = Vector3.one * keyPickupVisualScale;

            SpriteRenderer renderer = pickupObject.AddComponent<SpriteRenderer>();
            renderer.sprite = keyPickupSprite;
            renderer.sortingOrder = keyPickupSortingOrder;

            BoxCollider2D collider = pickupObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = keyPickupColliderSize / Mathf.Max(0.01f, keyPickupVisualScale);

            pickupObject.AddComponent<KeyPickup>();
        }

        [ContextMenu("Clear Key Pickups")]
        public void ClearKeyPickups()
        {
            Transform parent = GetKeyPickupParent();
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child == null || !child.name.StartsWith("KeyPickup_", StringComparison.Ordinal)) continue;

                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private bool TryGetSurfaceWorldPosition(int cellX, int surfaceY, out Vector3 worldPosition)
        {
            worldPosition = default;
            EnsureTilemapsAssigned();

            if (groundTilemap == null)
            {
                Debug.LogError("[BeginnerIslandMapBuilder] Ground tilemap is required for placement.");
                return false;
            }

            worldPosition = groundTilemap.GetCellCenterWorld(new Vector3Int(cellX, surfaceY, 0));
            worldPosition.y += groundTilemap.cellSize.y * 0.5f + spawnHeightOffset;
            return true;
        }

        private bool TryGetPickupWorldPosition(int cellX, int surfaceY, out Vector3 worldPosition)
        {
            return TryGetItemWorldPosition(cellX, surfaceY, healthPickupHeightOffset, out worldPosition);
        }

        private bool TryGetItemWorldPosition(int cellX, int surfaceY, float heightOffset, out Vector3 worldPosition)
        {
            worldPosition = default;
            EnsureTilemapsAssigned();

            if (groundTilemap == null)
            {
                Debug.LogError("[BeginnerIslandMapBuilder] Ground tilemap is required for item placement.");
                return false;
            }

            worldPosition = groundTilemap.GetCellCenterWorld(new Vector3Int(cellX, surfaceY, 0));
            worldPosition.y += groundTilemap.cellSize.y * 0.5f + heightOffset;
            return true;
        }

        private Transform GetHealthPickupParent()
        {
            if (healthPickupParent != null) return healthPickupParent;

            Transform existing = transform.Find("HealthPickups");
            if (existing != null)
            {
                healthPickupParent = existing;
                return healthPickupParent;
            }

            GameObject parentObject = new GameObject("HealthPickups");
            healthPickupParent = parentObject.transform;
            healthPickupParent.SetParent(transform, false);
            return healthPickupParent;
        }

        private Transform GetKeyPickupParent()
        {
            if (keyPickupParent != null) return keyPickupParent;

            Transform existing = transform.Find("KeyPickups");
            if (existing != null)
            {
                keyPickupParent = existing;
                return keyPickupParent;
            }

            GameObject parentObject = new GameObject("KeyPickups");
            keyPickupParent = parentObject.transform;
            keyPickupParent.SetParent(transform, false);
            return keyPickupParent;
        }

        private void EnsureTilemapsAssigned()
        {
            if (groundTilemap != null && decorationTilemap != null) return;

            Tilemap[] childTilemaps = GetComponentsInChildren<Tilemap>(true);
            foreach (Tilemap tilemap in childTilemaps)
            {
                if (tilemap == null) continue;

                if (groundTilemap == null && tilemap.GetComponent<TilemapCollider2D>() != null)
                {
                    groundTilemap = tilemap;
                    continue;
                }

                if (decorationTilemap == null && tilemap.name.IndexOf("Decoration", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    decorationTilemap = tilemap;
                }
            }
        }

        private void RefreshColliders()
        {
            TilemapCollider2D tilemapCollider = groundTilemap.GetComponent<TilemapCollider2D>();
            if (tilemapCollider != null)
            {
                tilemapCollider.ProcessTilemapChanges();
            }

            CompositeCollider2D composite = groundTilemap.GetComponent<CompositeCollider2D>();
            if (composite != null)
            {
                composite.GenerateGeometry();
            }
        }

        private bool SelectedMapUsesDecorations()
        {
            return selectedMap == MapTheme.FoggyForest;
        }

        private void GetPlacementForSelectedMap(out int spawnCellX, out int spawnSurfaceY, out int treasureCellX, out int treasureSurfaceY)
        {
            spawnCellX = PlayerSpawnCellX;
            spawnSurfaceY = PlayerSpawnSurfaceY;
            treasureCellX = TreasureCellX;
            treasureSurfaceY = TreasureSurfaceY;

            if (selectedMap == MapTheme.FoggyForest)
            {
                treasureCellX = FoggyForestTreasureCellX;
                treasureSurfaceY = FoggyForestTreasureSurfaceY;
            }
        }

        private Vector2Int[] GetHealthPickupPlacementsForSelectedMap()
        {
            switch (selectedMap)
            {
                case MapTheme.FoggyForest:
                    return new[]
                    {
                        new Vector2Int(34, -4),
                        new Vector2Int(52, -3),
                    };
                case MapTheme.VolcanoCave:
                    return new[]
                    {
                        new Vector2Int(24, -4),
                        new Vector2Int(76, -3),
                    };
                default:
                    return new[]
                    {
                        new Vector2Int(36, -4),
                        new Vector2Int(82, -3),
                    };
            }
        }

        private Vector2Int GetKeyPickupPlacementForSelectedMap()
        {
            switch (selectedMap)
            {
                case MapTheme.FoggyForest:
                    return new Vector2Int(46, -4);
                case MapTheme.VolcanoCave:
                    return new Vector2Int(56, -3);
                default:
                    return new Vector2Int(62, -3);
            }
        }

        private PlatformSegment[] GetLayoutForSelectedMap()
        {
            switch (selectedMap)
            {
                case MapTheme.FoggyForest:
                    return FoggyForestLayout;
                case MapTheme.VolcanoCave:
                    return VolcanoCaveLayout;
                default:
                    return DefaultLayout;
            }
        }
    }
}
