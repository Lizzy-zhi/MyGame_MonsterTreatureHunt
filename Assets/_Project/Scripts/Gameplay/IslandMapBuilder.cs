using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using MonsterTreasureHunt.CameraSystem;
using MonsterTreasureHunt.Gameplay;

namespace MonsterTreasureHunt.Levels
{
    [DisallowMultipleComponent]
    public class IslandMapBuilder : MonoBehaviour
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

        private struct ColoredPlacement
        {
            public TreasureKeyColor color;
            public int cellX;
            public int surfaceY;
        }

        [Header("Tilemaps")]
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap decorationTilemap;
        [SerializeField] private Tilemap backgroundWaterTilemap;
        [SerializeField] private Tilemap backgroundLavaTilemap;

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

        [Header("Stone Tiles")]
        [SerializeField] private TileBase stoneTopLeft;
        [SerializeField] private TileBase stoneTopCenter;
        [SerializeField] private TileBase stoneTopRight;
        [SerializeField] private TileBase stoneCenter;
        [SerializeField] private TileBase stoneBottom;
        [SerializeField] private TileBase stoneBottomLeft;
        [SerializeField] private TileBase stoneBottomRight;
        [SerializeField] private TileBase stoneLeftEdge;
        [SerializeField] private TileBase stoneRightEdge;

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

        [Header("Foggy Forest Water")]
        [SerializeField] private int foggyForestRiverTopY = -6;
        [SerializeField] private int foggyForestRiverDepth = 5;
        [SerializeField] private int backgroundWaterSortingOrder = -1;

        [Header("Volcano Decorations")]
        [SerializeField] private TileBase lavaTile;
        [SerializeField] private TileBase lavaTopTile;
        [SerializeField] private TileBase spikesTile;
        [SerializeField] private TileBase torchTile;
        [SerializeField] private int volcanoLavaTopY = -7;
        [SerializeField] private int volcanoLavaDepth = 4;

        [Header("Spike Hazards")]
        [SerializeField] private int spikeSortingOrder = 1;
        [SerializeField] private Vector2 spikeColliderSize = new Vector2(0.9f, 0.45f);
        [SerializeField] private Vector2 spikeColliderOffset = new Vector2(0f, 0.35f);
        [SerializeField] private Transform spikeHazardParent;

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
        [SerializeField] private Sprite redKeyPickupSprite;
        [SerializeField] private Sprite greenKeyPickupSprite;
        [SerializeField] private Sprite blueKeyPickupSprite;
        [SerializeField] private int keyPickupSortingOrder = 3;
        [SerializeField] private float keyPickupHeightOffset = 0.85f;
        [SerializeField] private float keyPickupVisualScale = 1.5f;
        [SerializeField] private Vector2 keyPickupColliderSize = new Vector2(1.1f, 1.1f);
        [SerializeField] private Transform keyPickupParent;

        [Header("Treasure Chests")]
        [SerializeField] private Sprite yellowChestBodySprite;
        [SerializeField] private Sprite redChestBodySprite;
        [SerializeField] private Sprite greenChestBodySprite;
        [SerializeField] private Sprite blueChestBodySprite;
        [SerializeField] private Sprite yellowChestLockSprite;
        [SerializeField] private Sprite redChestLockSprite;
        [SerializeField] private Sprite greenChestLockSprite;
        [SerializeField] private Sprite blueChestLockSprite;
        [SerializeField] private Sprite treasureUnlockEffectSprite;
        [SerializeField] private int treasureSortingOrder = 3;
        [SerializeField] private float treasureHeightOffset = 0.55f;
        [SerializeField] private float treasureVisualScale = 1.65f;
        [SerializeField] private Vector2 treasureColliderSize = new Vector2(1.25f, 1.15f);
        [SerializeField] private Vector2 treasureColliderOffset = new Vector2(0f, 0.05f);
        [SerializeField] private Transform treasureParent;

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
        private const int FoggyForestTreasureCellX = 88;
        private const int FoggyForestTreasureSurfaceY = 2;
        private const int VolcanoCaveTreasureCellX = 106;
        private const int VolcanoCaveTreasureSurfaceY = 2;
        private const int FoggyForestRiverEdgePadding = 8;
        private const int VolcanoLavaEdgePadding = 8;

        private static readonly ColoredPlacement[] BeginnerTreasurePlacements =
        {
            new() { color = TreasureKeyColor.Yellow, cellX = TreasureCellX, surfaceY = TreasureSurfaceY },
        };

        private static readonly ColoredPlacement[] FoggyForestTreasurePlacements =
        {
            // Act 2 canopy reward: visible from the trail, reached via upper stepping stones.
            new() { color = TreasureKeyColor.Yellow, cellX = 25, surfaceY = -1 },
            // Act 4 gated alcove: red key is on the risky high route over misty pits.
            new() { color = TreasureKeyColor.Red, cellX = 68, surfaceY = 0 },
            // Act 5 summit: final peak after the canopy climb.
            new() { color = TreasureKeyColor.Green, cellX = 88, surfaceY = 2 },
        };

        private static readonly ColoredPlacement[] VolcanoCaveTreasurePlacements =
        {
            new() { color = TreasureKeyColor.Yellow, cellX = 27, surfaceY = -1 },
            new() { color = TreasureKeyColor.Red, cellX = 71, surfaceY = 0 },
            new() { color = TreasureKeyColor.Green, cellX = VolcanoCaveTreasureCellX, surfaceY = VolcanoCaveTreasureSurfaceY },
        };

        private static readonly ColoredPlacement[] BeginnerKeyPlacements =
        {
            new() { color = TreasureKeyColor.Yellow, cellX = 66, surfaceY = -3 },
        };

        private static readonly ColoredPlacement[] FoggyForestKeyPlacements =
        {
            // Act 1 tutorial hop: first upper platform teaches vertical routing.
            new() { color = TreasureKeyColor.Yellow, cellX = -7, surfaceY = -3 },
            // Act 3 detour: narrow high stone over the mist pit.
            new() { color = TreasureKeyColor.Red, cellX = 45, surfaceY = -1 },
            // Act 4 second-tier jump platform: collected before the summit climb.
            new() { color = TreasureKeyColor.Green, cellX = 76, surfaceY = 1 },
        };

        private static readonly ColoredPlacement[] VolcanoCaveKeyPlacements =
        {
            new() { color = TreasureKeyColor.Yellow, cellX = -5, surfaceY = -3 },
            new() { color = TreasureKeyColor.Red, cellX = 45, surfaceY = -1 },
            // Upper route after the red chest: collected before the caldera crossing.
            new() { color = TreasureKeyColor.Green, cellX = 84, surfaceY = 1 },
        };

        private static readonly Vector2Int[] VolcanoSpikePlacements =
        {
            new(13, -2),
            new(18, -2),
            new(34, -2),
            new(50, -1),
            new(57, -1),
            new(68, 0),
            new(88, 1),
        };

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

            // Floating platform groups: short hops with alternating height to restore layered movement.
            new() { xMin = -11, xMax = -9, surfaceY = -3, depth = 1 },
            new() { xMin = -7, xMax = -5, surfaceY = -3, depth = 1 },
            new() { xMin = -2, xMax = 0, surfaceY = -3, depth = 1 },
            new() { xMin = 2, xMax = 4, surfaceY = -3, depth = 1 },
            new() { xMin = 7, xMax = 9, surfaceY = -3, depth = 1 },
            new() { xMin = 13, xMax = 15, surfaceY = -2, depth = 1 },
            new() { xMin = 17, xMax = 19, surfaceY = -2, depth = 1 },
            new() { xMin = 23, xMax = 26, surfaceY = -2, depth = 1 },
            new() { xMin = 28, xMax = 30, surfaceY = -2, depth = 1 },
            new() { xMin = 33, xMax = 35, surfaceY = -2, depth = 1 },
            new() { xMin = 37, xMax = 39, surfaceY = -2, depth = 1 },
            new() { xMin = 42, xMax = 44, surfaceY = -2, depth = 1 },
            new() { xMin = 46, xMax = 48, surfaceY = -2, depth = 1 },
            new() { xMin = 51, xMax = 53, surfaceY = -1, depth = 1 },
            new() { xMin = 55, xMax = 57, surfaceY = -1, depth = 1 },
            new() { xMin = 60, xMax = 62, surfaceY = -1, depth = 1 },
            new() { xMin = 64, xMax = 66, surfaceY = -1, depth = 1 },
            new() { xMin = 69, xMax = 71, surfaceY = -1, depth = 1 },
            new() { xMin = 73, xMax = 75, surfaceY = -1, depth = 1 },
            new() { xMin = 78, xMax = 80, surfaceY = -1, depth = 1 },
            new() { xMin = 82, xMax = 84, surfaceY = -1, depth = 1 },
            new() { xMin = 87, xMax = 89, surfaceY = -1, depth = 1 },
            new() { xMin = 91, xMax = 93, surfaceY = -1, depth = 1 },
        };

        private static readonly PlatformSegment[] FoggyForestLayout =
        {
            // Act 1 - Forest trail: safe spawn, readable gaps, first upper hops.
            new() { xMin = -28, xMax = -16, surfaceY = -5, depth = 6 },
            new() { xMin = -14, xMax = -4, surfaceY = -5, depth = 6 },
            new() { xMin = -2, xMax = 8, surfaceY = -5, depth = 6 },
            new() { xMin = -11, xMax = -9, surfaceY = -3, depth = 1 },
            new() { xMin = -8, xMax = -6, surfaceY = -3, depth = 1 },
            new() { xMin = -3, xMax = -1, surfaceY = -3, depth = 1 },
            new() { xMin = 1, xMax = 3, surfaceY = -2, depth = 1 },
            new() { xMin = 5, xMax = 7, surfaceY = -2, depth = 1 },

            // Act 2 - Canopy climb: floor rises while an upper route rewards exploration.
            new() { xMin = 10, xMax = 20, surfaceY = -4, depth = 5 },
            new() { xMin = 22, xMax = 32, surfaceY = -4, depth = 5 },
            new() { xMin = 12, xMax = 14, surfaceY = -2, depth = 1 },
            new() { xMin = 16, xMax = 18, surfaceY = -2, depth = 1 },
            new() { xMin = 20, xMax = 22, surfaceY = -1, depth = 1 },
            new() { xMin = 24, xMax = 26, surfaceY = -1, depth = 1 },
            new() { xMin = 28, xMax = 30, surfaceY = -1, depth = 1 },

            // Act 3 - Misty hollow: lower sunken path with a high-risk upper detour.
            new() { xMin = 34, xMax = 42, surfaceY = -5, depth = 6 },
            new() { xMin = 44, xMax = 52, surfaceY = -5, depth = 6 },
            new() { xMin = 54, xMax = 62, surfaceY = -4, depth = 5 },
            new() { xMin = 35, xMax = 37, surfaceY = -2, depth = 1 },
            new() { xMin = 39, xMax = 41, surfaceY = -2, depth = 1 },
            new() { xMin = 44, xMax = 46, surfaceY = -1, depth = 1 },
            new() { xMin = 48, xMax = 50, surfaceY = -1, depth = 1 },
            new() { xMin = 51, xMax = 59, surfaceY = -1, depth = 1 },

            // Act 4 - Forest gate: stepped route up to the red chest alcove.
            new() { xMin = 64, xMax = 72, surfaceY = -4, depth = 5 },
            new() { xMin = 74, xMax = 82, surfaceY = -3, depth = 5 },
            new() { xMin = 61, xMax = 63, surfaceY = -2, depth = 1 },
            new() { xMin = 65, xMax = 67, surfaceY = -1, depth = 1 },
            new() { xMin = 68, xMax = 70, surfaceY = 0, depth = 1 },
            new() { xMin = 71, xMax = 73, surfaceY = 0, depth = 1 },
            new() { xMin = 75, xMax = 77, surfaceY = 1, depth = 1 },
            new() { xMin = 79, xMax = 81, surfaceY = 1, depth = 1 },

            // Act 5 - Summit: short rest ledge then the final peak.
            new() { xMin = 84, xMax = 90, surfaceY = -2, depth = 5 },
            new() { xMin = 83, xMax = 85, surfaceY = 1, depth = 1 },
            new() { xMin = 87, xMax = 89, surfaceY = 2, depth = 1 },
        };

        private static readonly PlatformSegment[] VolcanoCaveLayout =
        {
            // Act 1 - Cave mouth: wider gaps than Foggy Forest.
            new() { xMin = -28, xMax = -16, surfaceY = -5, depth = 8 },
            new() { xMin = -13, xMax = -3, surfaceY = -5, depth = 8 },
            new() { xMin = 0, xMax = 8, surfaceY = -5, depth = 8 },
            new() { xMin = -10, xMax = -8, surfaceY = -3, depth = 1 },
            new() { xMin = -6, xMax = -4, surfaceY = -3, depth = 1 },
            new() { xMin = -1, xMax = 1, surfaceY = -2, depth = 1 },

            // Act 2 - First lava flats.
            new() { xMin = 11, xMax = 18, surfaceY = -4, depth = 7 },
            new() { xMin = 21, xMax = 28, surfaceY = -4, depth = 7 },
            new() { xMin = 12, xMax = 14, surfaceY = -2, depth = 1 },
            new() { xMin = 17, xMax = 19, surfaceY = -2, depth = 1 },
            new() { xMin = 22, xMax = 24, surfaceY = -1, depth = 1 },
            new() { xMin = 26, xMax = 28, surfaceY = -1, depth = 1 },

            // Act 3 - Deep heat: sunken floor and long upper detour.
            new() { xMin = 32, xMax = 40, surfaceY = -5, depth = 8 },
            new() { xMin = 43, xMax = 51, surfaceY = -5, depth = 8 },
            new() { xMin = 54, xMax = 62, surfaceY = -4, depth = 7 },
            new() { xMin = 33, xMax = 35, surfaceY = -2, depth = 1 },
            new() { xMin = 38, xMax = 40, surfaceY = -2, depth = 1 },
            new() { xMin = 44, xMax = 46, surfaceY = -1, depth = 1 },
            new() { xMin = 49, xMax = 51, surfaceY = -1, depth = 1 },
            new() { xMin = 53, xMax = 61, surfaceY = -1, depth = 1 },

            // Act 4 - Magma gate: stepped climb to the red chest alcove.
            new() { xMin = 66, xMax = 74, surfaceY = -4, depth = 7 },
            new() { xMin = 78, xMax = 86, surfaceY = -3, depth = 7 },
            new() { xMin = 63, xMax = 65, surfaceY = -2, depth = 1 },
            new() { xMin = 67, xMax = 69, surfaceY = -1, depth = 1 },
            new() { xMin = 70, xMax = 72, surfaceY = 0, depth = 1 },
            new() { xMin = 75, xMax = 77, surfaceY = 0, depth = 1 },
            new() { xMin = 79, xMax = 81, surfaceY = 0, depth = 1 },
            new() { xMin = 83, xMax = 85, surfaceY = 1, depth = 1 },

            // Act 5 - Caldera summit.
            new() { xMin = 90, xMax = 98, surfaceY = -2, depth = 7 },
            new() { xMin = 102, xMax = 110, surfaceY = -1, depth = 7 },
            new() { xMin = 87, xMax = 89, surfaceY = 1, depth = 1 },
            new() { xMin = 88, xMax = 88, surfaceY = -3, depth = 1 },
            new() { xMin = 91, xMax = 93, surfaceY = 1, depth = 1 },
            new() { xMin = 96, xMax = 98, surfaceY = 1, depth = 1 },
            new() { xMin = 99, xMax = 100, surfaceY = 1, depth = 1 },
            new() { xMin = 101, xMax = 103, surfaceY = 2, depth = 1 },
            new() { xMin = 105, xMax = 107, surfaceY = 2, depth = 1 },
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
            ApplyCameraTheme();
        }

        [ContextMenu("Build Selected Map Ground")]
        public void BuildGround()
        {
            EnsureTilemapsAssigned();

            if (groundTilemap == null)
            {
                Debug.LogError("[IslandMapBuilder] Ground tilemap is not assigned.");
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

            if (selectedMap == MapTheme.BeginnerIsland)
            {
                PlaceDecorations();
            }
            else if (selectedMap == MapTheme.FoggyForest)
            {
                PlaceFoggyForestDecorations();
            }
            else if (selectedMap == MapTheme.VolcanoCave)
            {
                PlaceVolcanoCaveDecorations();
            }
        }

        [ContextMenu("Clear Map Decorations")]
        public void ClearDecorations()
        {
            EnsureTilemapsAssigned();
            if (decorationTilemap == null) return;

            decorationTilemap.ClearAllTiles();

            if (TryGetBackgroundWaterTilemap(out Tilemap riverTilemap))
            {
                riverTilemap.ClearAllTiles();
            }

            if (TryGetBackgroundLavaTilemap(out Tilemap lavaTilemap))
            {
                lavaTilemap.ClearAllTiles();
            }
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
            if (selectedMap == MapTheme.VolcanoCave)
            {
                return PickTileFromTheme(
                    isTop,
                    isBottom,
                    isLeft,
                    isRight,
                    singleColumn,
                    stoneTopLeft != null ? stoneTopLeft : topLeft,
                    stoneTopCenter != null ? stoneTopCenter : topCenter,
                    stoneTopRight != null ? stoneTopRight : topRight,
                    stoneCenter != null ? stoneCenter : center,
                    stoneBottom != null ? stoneBottom : bottom,
                    stoneBottomLeft != null ? stoneBottomLeft : bottomLeft,
                    stoneBottomRight != null ? stoneBottomRight : bottomRight,
                    stoneLeftEdge != null ? stoneLeftEdge : leftEdge,
                    stoneRightEdge != null ? stoneRightEdge : rightEdge);
            }

            return PickTileFromTheme(
                isTop,
                isBottom,
                isLeft,
                isRight,
                singleColumn,
                topLeft,
                topCenter,
                topRight,
                center,
                bottom,
                bottomLeft,
                bottomRight,
                leftEdge,
                rightEdge);
        }

        private static TileBase PickTileFromTheme(
            bool isTop,
            bool isBottom,
            bool isLeft,
            bool isRight,
            bool singleColumn,
            TileBase themeTopLeft,
            TileBase themeTopCenter,
            TileBase themeTopRight,
            TileBase themeCenter,
            TileBase themeBottom,
            TileBase themeBottomLeft,
            TileBase themeBottomRight,
            TileBase themeLeftEdge,
            TileBase themeRightEdge)
        {
            if (isTop)
            {
                if (singleColumn) return themeTopCenter;
                if (isLeft) return themeTopLeft;
                if (isRight) return themeTopRight;
                return themeTopCenter;
            }

            if (isBottom)
            {
                if (singleColumn) return themeBottom;
                if (isLeft) return themeBottomLeft;
                if (isRight) return themeBottomRight;
                return themeBottom;
            }

            if (singleColumn) return themeCenter;
            if (isLeft) return themeLeftEdge;
            if (isRight) return themeRightEdge;
            return themeCenter;
        }

        private void PlaceDecorations()
        {
            if (decorationTilemap == null) return;

            SetSurfaceDecoration(-27, -5, signTile);
            SetSurfaceDecoration(-21, -5, bushTile);
            SetSurfaceDecoration(-8, -5, rockTile);
            SetSurfaceDecoration(8, -5, bushTile);
            SetSurfaceDecoration(18, -4, bushTile);
            SetSurfaceDecoration(34, -4, rockTile);
            SetSurfaceDecoration(48, -4, bushTile);
            SetSurfaceDecoration(72, -3, rockTile);
            SetSurfaceDecoration(91, -3, signTile);

            BuildWaterStrip(-36, -29, -7);
            BuildWaterStrip(105, 112, -5);
        }

        private void PlaceFoggyForestDecorations()
        {
            if (decorationTilemap == null) return;

            BuildFoggyForestFullRiver();

            // Act 1 trail markers.
            SetSurfaceDecorationOnOpenGround(-27, -5, signTile);
            SetSurfaceDecorationOnOpenGround(-22, -5, bushTile);
            SetSurfaceDecorationOnOpenGround(-14, -5, rockTile);
            SetSurfaceDecorationOnOpenGround(-10, -3, mushroomBrownTile);
            SetSurfaceDecorationOnOpenGround(-5, -3, mushroomRedTile);
            SetSurfaceDecorationOnOpenGround(2, -2, bushTile);

            // Act 2 canopy framing.
            SetSurfaceDecorationOnOpenGround(11, -4, bushTile);
            SetSurfaceDecorationOnOpenGround(18, -4, rockTile);
            SetSurfaceDecorationOnOpenGround(25, -1, mushroomBrownTile);
            SetSurfaceDecorationOnOpenGround(29, -1, mushroomRedTile);

            // Act 3 mist pits and risky detour cues.
            BuildFoggyForestGapBridge(21);
            SetSurfaceDecorationOnOpenGround(40, -2, mushroomBrownTile);
            SetSurfaceDecorationOnOpenGround(45, -1, rockTile);
            SetSurfaceDecorationOnOpenGround(58, -4, bushTile);

            // Act 4 gate leading to the red chest alcove.
            SetSurfaceDecorationOnOpenGround(64, -4, fenceTile);
            SetSurfaceDecorationOnOpenGround(68, 0, fenceTile);
            SetSurfaceDecorationOnOpenGround(69, 0, fenceTile);
            SetSurfaceDecorationOnOpenGround(70, 0, fenceTile);
            SetSurfaceDecorationOnOpenGround(76, 1, mushroomRedTile);

            // Act 5 summit approach.
            SetSurfaceDecorationOnOpenGround(80, -3, signTile);
            SetSurfaceDecorationOnOpenGround(84, 1, bushTile);
            SetSurfaceDecorationOnOpenGround(87, 2, mushroomRedTile);
            SetSurfaceDecorationOnOpenGround(89, 2, mushroomBrownTile);
        }

        private void BuildFoggyForestGapWater(int gapX)
        {
            if (HasOpenGroundAt(gapX)) return;

            BuildFoggyForestRiverStrip(gapX, gapX);
        }

        private void BuildFoggyForestGapBridge(int gapX)
        {
            BuildGapBridge(gapX, bridgeTile);
        }

        private void BuildFoggyForestFullRiver()
        {
            Tilemap riverTilemap = GetOrCreateBackgroundWaterTilemap();
            if (riverTilemap == null) return;

            GetLayoutHorizontalBounds(FoggyForestLayout, out int minX, out int maxX);
            BuildWaterStrip(
                riverTilemap,
                minX - FoggyForestRiverEdgePadding,
                maxX + FoggyForestRiverEdgePadding,
                foggyForestRiverTopY,
                foggyForestRiverDepth);
        }

        private void BuildFoggyForestRiverStrip(int xMin, int xMax)
        {
            Tilemap riverTilemap = GetOrCreateBackgroundWaterTilemap();
            if (riverTilemap == null) return;

            BuildWaterStrip(riverTilemap, xMin, xMax, foggyForestRiverTopY, foggyForestRiverDepth);
        }

        private void PlaceVolcanoCaveDecorations()
        {
            if (decorationTilemap == null) return;

            BuildVolcanoFullLava();

            SetSurfaceDecorationOnOpenGround(-25, -5, torchTile);
            SetSurfaceDecorationOnOpenGround(-4, -5, torchTile);
            SetSurfaceDecorationOnOpenGround(14, -4, torchTile);
            SetSurfaceDecorationOnOpenGround(36, -5, torchTile);
            SetSurfaceDecorationOnOpenGround(58, -4, torchTile);
            SetSurfaceDecorationOnOpenGround(70, 0, torchTile);
            SetSurfaceDecorationOnOpenGround(94, -2, torchTile);
            SetSurfaceDecorationOnOpenGround(106, 2, torchTile);
        }

        private void BuildVolcanoFullLava()
        {
            Tilemap lavaTilemap = GetOrCreateBackgroundLavaTilemap();
            if (lavaTilemap == null) return;

            GetLayoutHorizontalBounds(VolcanoCaveLayout, out int minX, out int maxX);
            BuildLavaStrip(
                lavaTilemap,
                minX - VolcanoLavaEdgePadding,
                maxX + VolcanoLavaEdgePadding,
                volcanoLavaTopY,
                volcanoLavaDepth);
        }

        private void BuildVolcanoGapLava(int gapXMin, int gapXMax = int.MinValue)
        {
            if (gapXMax == int.MinValue)
            {
                gapXMax = gapXMin;
            }

            BuildVolcanoLavaInOpenPitColumns(gapXMin, gapXMax, volcanoLavaTopY, volcanoLavaDepth);
        }

        private void BuildVolcanoBridgedPitLava(int gapXMin, int gapXMax)
        {
            BuildVolcanoLavaInOpenPitColumns(gapXMin, gapXMax, volcanoLavaTopY, volcanoLavaDepth);
        }

        private bool TryGetGapEdgeFloatingSurfaces(int gapXMin, int gapXMax, out int leftSurfaceY, out int rightSurfaceY)
        {
            leftSurfaceY = int.MinValue;
            rightSurfaceY = int.MinValue;

            bool hasLeft = TryGetHighestFloatingSurfaceAt(gapXMin - 1, out leftSurfaceY);
            bool hasRight = TryGetHighestFloatingSurfaceAt(gapXMax + 1, out rightSurfaceY);
            return hasLeft && hasRight;
        }

        private bool TryGetGapEdgeMainFloorSurfaces(int gapXMin, int gapXMax, out int leftSurfaceY, out int rightSurfaceY)
        {
            leftSurfaceY = int.MinValue;
            rightSurfaceY = int.MinValue;

            if (!TryGetMainFloorSurfaceAt(gapXMin - 1, out leftSurfaceY)) return false;

            for (int cellX = gapXMax + 1; cellX <= gapXMax + 8; cellX++)
            {
                if (TryGetMainFloorSurfaceAt(cellX, out rightSurfaceY)) return true;
            }

            return false;
        }

        private bool TryGetMainFloorSurfaceAt(int cellX, out int surfaceY)
        {
            surfaceY = int.MaxValue;
            bool found = false;
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (segment.depth <= 1) continue;
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (!IsSurfaceOpen(cellX, segment.surfaceY)) continue;

                if (!found || segment.surfaceY < surfaceY)
                {
                    surfaceY = segment.surfaceY;
                    found = true;
                }
            }

            return found;
        }

        private bool TryGetHighestFloatingSurfaceAt(int cellX, out int surfaceY)
        {
            surfaceY = int.MinValue;
            bool found = false;
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (segment.depth != 1) continue;
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (!IsSurfaceOpen(cellX, segment.surfaceY)) continue;

                if (!found || segment.surfaceY > surfaceY)
                {
                    surfaceY = segment.surfaceY;
                    found = true;
                }
            }

            return found;
        }

        private void BuildBoundaryLava(int xMin, int xMax, int adjacentSurfaceY)
        {
            BuildLavaStrip(xMin, xMax, volcanoLavaTopY, volcanoLavaDepth);
        }

        private void SetGroundDecorationOnOpenGround(int cellX, int preferredSurfaceY, TileBase tile)
        {
            if (tile == null || decorationTilemap == null) return;

            int surfaceY = ResolveOpenSurfaceY(cellX, preferredSurfaceY);
            if (!IsSurfaceOpen(cellX, surfaceY)) return;

            SetGroundDecoration(cellX, surfaceY, tile);
        }

        private void BuildWaterStrip(int xMin, int xMax, int topY, int depth = 2)
        {
            BuildWaterStrip(decorationTilemap, xMin, xMax, topY, depth);
        }

        private void BuildWaterStrip(Tilemap targetTilemap, int xMin, int xMax, int topY, int depth = 2)
        {
            if (targetTilemap == null) return;

            BuildDecorationLine(targetTilemap, xMin, xMax, topY, waterTopTile);

            for (int layer = 1; layer <= Mathf.Max(1, depth - 1); layer++)
            {
                BuildDecorationLine(targetTilemap, xMin, xMax, topY - layer, waterTile);
            }
        }

        private void BuildLavaStrip(int xMin, int xMax, int topY, int depth = 2)
        {
            BuildLavaStrip(decorationTilemap, xMin, xMax, topY, depth);
        }

        private void BuildLavaStrip(Tilemap targetTilemap, int xMin, int xMax, int topY, int depth = 2)
        {
            if (targetTilemap == null) return;

            BuildDecorationLine(targetTilemap, xMin, xMax, topY, lavaTopTile);

            for (int layer = 1; layer <= Mathf.Max(1, depth - 1); layer++)
            {
                BuildDecorationLine(targetTilemap, xMin, xMax, topY - layer, lavaTile);
            }
        }

        private void BuildVolcanoLavaInOpenPitColumns(int xMin, int xMax, int topY, int depth)
        {
            int runStart = int.MinValue;

            for (int x = xMin; x <= xMax; x++)
            {
                if (HasMainFloorAt(x))
                {
                    if (runStart != int.MinValue)
                    {
                        BuildLavaStrip(runStart, x - 1, topY, depth);
                        runStart = int.MinValue;
                    }

                    continue;
                }

                if (runStart == int.MinValue)
                {
                    runStart = x;
                }
            }

            if (runStart != int.MinValue)
            {
                BuildLavaStrip(runStart, xMax, topY, depth);
            }
        }

        private void BuildDecorationLine(int xMin, int xMax, int y, TileBase tile)
        {
            BuildDecorationLine(decorationTilemap, xMin, xMax, y, tile);
        }

        private void BuildDecorationLine(Tilemap targetTilemap, int xMin, int xMax, int y, TileBase tile)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                SetTile(targetTilemap, x, y, tile);
            }
        }

        private void SetSurfaceDecoration(int x, int surfaceY, TileBase tile)
        {
            SetDecoration(x, surfaceY + 1, tile);
        }

        private void SetSurfaceDecorationOnOpenGround(int cellX, int preferredSurfaceY, TileBase tile)
        {
            if (tile == null || decorationTilemap == null) return;

            int surfaceY = ResolveOpenSurfaceY(cellX, preferredSurfaceY);
            if (!IsSurfaceOpen(cellX, surfaceY)) return;

            SetSurfaceDecoration(cellX, surfaceY, tile);
        }

        private void BuildGapWater(int gapX)
        {
            if (!TryGetAdjacentPlatformSurfaces(gapX, out int leftSurfaceY, out int rightSurfaceY)) return;

            int pitSurfaceY = Mathf.Min(leftSurfaceY, rightSurfaceY);
            BuildWaterStrip(gapX, gapX, pitSurfaceY - 2);
        }

        private void BuildBoundaryWater(int xMin, int xMax, int adjacentSurfaceY)
        {
            BuildWaterStrip(xMin, xMax, adjacentSurfaceY - 2);
        }

        private bool HasOpenGroundAt(int cellX)
        {
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (IsSurfaceOpen(cellX, segment.surfaceY)) return true;
            }

            return false;
        }

        private bool HasMainFloorAt(int cellX)
        {
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (segment.depth <= 1) continue;
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (IsSurfaceOpen(cellX, segment.surfaceY)) return true;
            }

            return false;
        }

        private void BuildGapBridge(int gapX, TileBase tile)
        {
            if (tile == null || !TryGetAdjacentPlatformSurfaces(gapX, out int leftSurfaceY, out int rightSurfaceY)) return;

            int bridgeSurfaceY = Mathf.Max(leftSurfaceY, rightSurfaceY);
            SetSurfaceDecoration(gapX, bridgeSurfaceY, tile);
        }

        private bool TryGetAdjacentPlatformSurfaces(int gapX, out int leftSurfaceY, out int rightSurfaceY)
        {
            leftSurfaceY = int.MinValue;
            rightSurfaceY = int.MinValue;

            if (!TryGetHighestOpenSurfaceAt(gapX - 1, ref leftSurfaceY)) return false;
            if (!TryGetHighestOpenSurfaceAt(gapX + 1, ref rightSurfaceY)) return false;

            return true;
        }

        private bool TryGetHighestOpenSurfaceAt(int cellX, ref int highestSurfaceY)
        {
            bool found = false;
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (!IsSurfaceOpen(cellX, segment.surfaceY)) continue;

                if (!found || segment.surfaceY > highestSurfaceY)
                {
                    highestSurfaceY = segment.surfaceY;
                    found = true;
                }
            }

            return found;
        }

        private void SetGroundDecoration(int x, int surfaceY, TileBase tile)
        {
            SetTile(decorationTilemap, x, surfaceY, tile);
        }

        private void SetDecoration(int x, int y, TileBase tile)
        {
            SetTile(decorationTilemap, x, y, tile);
        }

        private static void SetTile(Tilemap targetTilemap, int x, int y, TileBase tile)
        {
            if (tile == null || targetTilemap == null) return;

            targetTilemap.SetTile(new Vector3Int(x, y, 0), tile);
        }

        [ContextMenu("Place Selected Map Gameplay Objects")]
        public void PlaceGameplayObjects()
        {
            PlacePlayerSpawn();
            PlaceTreasure();
            BuildHealthPickups();
            BuildKeyPickups();
            BuildSpikeHazards();
        }

        public void SelectMap(MapTheme map)
        {
            selectedMap = map;
        }

        private void ApplyCameraTheme()
        {
            CameraFollow2D cameraFollow = FindObjectOfType<CameraFollow2D>();
            if (cameraFollow == null) return;

            cameraFollow.ApplyMapTheme(selectedMap);
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
            BuildTreasures();
        }

        [ContextMenu("Build Treasures")]
        public void BuildTreasures()
        {
            ClearGeneratedTreasures();

            ColoredPlacement[] placements = GetTreasurePlacementsForSelectedMap();
            if (placements.Length == 0) return;

            if (treasure != null)
            {
                treasure.gameObject.SetActive(false);
            }

            for (int i = 0; i < placements.Length; i++)
            {
                ColoredPlacement placement = placements[i];
                if (!TryGetItemWorldPosition(placement.cellX, placement.surfaceY, treasureHeightOffset, out Vector3 worldPosition)) continue;

                CreateTreasure(i + 1, placement.color, worldPosition);
            }
        }

        [ContextMenu("Clear Generated Treasures")]
        public void ClearGeneratedTreasures()
        {
            Transform parent = GetTreasureParent();
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child == null || !child.name.StartsWith("TreasureChest_", StringComparison.Ordinal)) continue;

                DestroyGeneratedChild(child.gameObject);
            }
        }

        private void CreateTreasure(int index, TreasureKeyColor color, Vector3 worldPosition)
        {
            GameObject treasureObject = new GameObject($"TreasureChest_{TreasureKeyColorUtility.GetDisplayName(color)}_{index:00}");
            treasureObject.tag = "Treasure";
            treasureObject.transform.SetParent(GetTreasureParent(), true);
            treasureObject.transform.position = worldPosition;
            treasureObject.transform.localScale = Vector3.one * treasureVisualScale;

            SpriteRenderer bodyRenderer = treasureObject.AddComponent<SpriteRenderer>();
            Sprite lockSprite = GetChestLockSprite(color);
            Sprite chestSprite = lockSprite != null ? lockSprite : GetChestBodySprite(color);
            bodyRenderer.sprite = chestSprite;
            bodyRenderer.sortingOrder = treasureSortingOrder;

            BoxCollider2D collider = treasureObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = treasureColliderSize / Mathf.Max(0.01f, treasureVisualScale);
            collider.offset = treasureColliderOffset / Mathf.Max(0.01f, treasureVisualScale);

            TreasureCollectible collectible = treasureObject.AddComponent<TreasureCollectible>();
            collectible.ConfigureKeyRequirement(color);

            collectible.ConfigureTrigger(treasureColliderSize / Mathf.Max(0.01f, treasureVisualScale), treasureColliderOffset / Mathf.Max(0.01f, treasureVisualScale));
            collectible.ConfigureUnlockEffect(treasureUnlockEffectSprite);
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
                    DestroyGeneratedChild(child.gameObject);
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
            BuildKeyPickups();
        }

        [ContextMenu("Build Key Pickups")]
        public void BuildKeyPickups()
        {
            ClearKeyPickups();

            ColoredPlacement[] placements = GetKeyPlacementsForSelectedMap();
            for (int i = 0; i < placements.Length; i++)
            {
                ColoredPlacement placement = placements[i];
                Sprite keySprite = GetKeyPickupSprite(placement.color);
                if (keySprite == null) continue;
                if (!TryGetItemWorldPosition(placement.cellX, placement.surfaceY, keyPickupHeightOffset, out Vector3 worldPosition)) continue;

                CreateKeyPickup(i + 1, placement.color, keySprite, worldPosition);
            }
        }

        private void CreateKeyPickup(int index, TreasureKeyColor color, Sprite keySprite, Vector3 worldPosition)
        {
            string colorName = TreasureKeyColorUtility.GetDisplayName(color);
            GameObject pickupObject = new GameObject($"KeyPickup_{colorName}_{index:00}");
            pickupObject.transform.SetParent(GetKeyPickupParent(), true);
            pickupObject.transform.position = worldPosition;
            pickupObject.transform.localScale = Vector3.one * keyPickupVisualScale;

            SpriteRenderer renderer = pickupObject.AddComponent<SpriteRenderer>();
            renderer.sprite = keySprite;
            renderer.sortingOrder = keyPickupSortingOrder;

            BoxCollider2D collider = pickupObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = keyPickupColliderSize / Mathf.Max(0.01f, keyPickupVisualScale);

            KeyPickup keyPickup = pickupObject.AddComponent<KeyPickup>();
            keyPickup.KeyColor = color;
        }

        [ContextMenu("Clear Key Pickups")]
        public void ClearKeyPickups()
        {
            Transform parent = GetKeyPickupParent();
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child == null || !child.name.StartsWith("KeyPickup_", StringComparison.Ordinal)) continue;

                DestroyGeneratedChild(child.gameObject);
            }
        }

        [ContextMenu("Build Spike Hazards")]
        public void BuildSpikeHazards()
        {
            ClearSpikeHazards();

            if (selectedMap != MapTheme.VolcanoCave) return;

            Sprite spikeSprite = GetSpikeSprite();
            if (spikeSprite == null) return;

            for (int i = 0; i < VolcanoSpikePlacements.Length; i++)
            {
                Vector2Int placement = VolcanoSpikePlacements[i];
                if (!IsUpperRouteSurface(placement.x, placement.y)) continue;
                if (!TryGetItemWorldPosition(placement.x, placement.y, 0.02f, out Vector3 worldPosition)) continue;

                CreateSpikeHazard(i + 1, placement.x, placement.y, spikeSprite, worldPosition);
            }
        }

        [ContextMenu("Clear Spike Hazards")]
        public void ClearSpikeHazards()
        {
            Transform parent = GetSpikeHazardParent();
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child == null || !child.name.StartsWith("SpikeHazard_", StringComparison.Ordinal)) continue;

                DestroyGeneratedChild(child.gameObject);
            }
        }

        private void CreateSpikeHazard(int index, int cellX, int surfaceY, Sprite spikeSprite, Vector3 worldPosition)
        {
            GameObject spikeObject = new GameObject($"SpikeHazard_{cellX}_{surfaceY}_{index:00}");
            spikeObject.transform.SetParent(GetSpikeHazardParent(), true);
            spikeObject.transform.position = worldPosition;

            SpriteRenderer renderer = spikeObject.AddComponent<SpriteRenderer>();
            renderer.sprite = spikeSprite;
            renderer.sortingOrder = spikeSortingOrder;

            BoxCollider2D collider = spikeObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = spikeColliderSize;
            collider.offset = spikeColliderOffset;

            spikeObject.AddComponent<SpikeHazard>();
        }

        private Sprite GetSpikeSprite()
        {
            if (spikesTile is Tile tile && tile.sprite != null)
            {
                return tile.sprite;
            }

            return null;
        }

        private bool IsUpperRouteSurface(int cellX, int surfaceY)
        {
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (segment.depth != 1 || segment.surfaceY != surfaceY) continue;
                if (IsSurfaceOpen(cellX, surfaceY)) return true;
            }

            return false;
        }

        private Transform GetSpikeHazardParent()
        {
            if (spikeHazardParent != null) return spikeHazardParent;

            Transform existing = transform.Find("SpikeHazards");
            if (existing != null)
            {
                spikeHazardParent = existing;
                return spikeHazardParent;
            }

            GameObject parentObject = new GameObject("SpikeHazards");
            spikeHazardParent = parentObject.transform;
            spikeHazardParent.SetParent(transform, false);
            return spikeHazardParent;
        }

        private void DestroyGeneratedChild(GameObject child)
        {
            if (child == null) return;

            child.SetActive(false);
            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }
        }

        private bool TryGetSurfaceWorldPosition(int cellX, int surfaceY, out Vector3 worldPosition)
        {
            worldPosition = default;
            EnsureTilemapsAssigned();

            if (groundTilemap == null)
            {
                Debug.LogError("[IslandMapBuilder] Ground tilemap is required for placement.");
                return false;
            }

            surfaceY = ResolveOpenSurfaceY(cellX, surfaceY);
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
                Debug.LogError("[IslandMapBuilder] Ground tilemap is required for item placement.");
                return false;
            }

            surfaceY = ResolveOpenSurfaceY(cellX, surfaceY);
            worldPosition = groundTilemap.GetCellCenterWorld(new Vector3Int(cellX, surfaceY, 0));
            worldPosition.y += groundTilemap.cellSize.y * 0.5f + heightOffset;
            return true;
        }

        private int ResolveOpenSurfaceY(int cellX, int preferredSurfaceY)
        {
            int fallbackSurfaceY = int.MinValue;
            PlatformSegment[] layout = GetLayoutForSelectedMap();

            foreach (PlatformSegment segment in layout)
            {
                if (cellX < segment.xMin || cellX > segment.xMax) continue;
                if (!IsSurfaceOpen(cellX, segment.surfaceY)) continue;

                if (segment.surfaceY == preferredSurfaceY)
                {
                    return preferredSurfaceY;
                }

                if (fallbackSurfaceY == int.MinValue || segment.surfaceY > fallbackSurfaceY)
                {
                    fallbackSurfaceY = segment.surfaceY;
                }
            }

            return fallbackSurfaceY != int.MinValue ? fallbackSurfaceY : preferredSurfaceY;
        }

        private bool IsSurfaceOpen(int cellX, int surfaceY)
        {
            if (groundTilemap == null) return true;

            Vector3Int surfaceCell = new Vector3Int(cellX, surfaceY, 0);
            Vector3Int aboveCell = new Vector3Int(cellX, surfaceY + 1, 0);
            return groundTilemap.HasTile(surfaceCell) && !groundTilemap.HasTile(aboveCell);
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

        private Transform GetTreasureParent()
        {
            if (treasureParent != null) return treasureParent;

            Transform existing = transform.Find("Treasures");
            if (existing != null)
            {
                treasureParent = existing;
                return treasureParent;
            }

            GameObject parentObject = new GameObject("Treasures");
            treasureParent = parentObject.transform;
            treasureParent.SetParent(transform, false);
            return treasureParent;
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

        private bool TryGetBackgroundWaterTilemap(out Tilemap tilemap)
        {
            if (backgroundWaterTilemap != null)
            {
                EnsureBackgroundWaterTilemapSettings(backgroundWaterTilemap);
                tilemap = backgroundWaterTilemap;
                return true;
            }

            Tilemap[] childTilemaps = GetComponentsInChildren<Tilemap>(true);
            foreach (Tilemap childTilemap in childTilemaps)
            {
                if (childTilemap == null) continue;
                if (childTilemap.name.IndexOf("BackgroundWater", StringComparison.OrdinalIgnoreCase) < 0 &&
                    childTilemap.name.IndexOf("River", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                backgroundWaterTilemap = childTilemap;
                EnsureBackgroundWaterTilemapSettings(backgroundWaterTilemap);
                tilemap = backgroundWaterTilemap;
                return true;
            }

            tilemap = null;
            return false;
        }

        private bool TryGetBackgroundLavaTilemap(out Tilemap tilemap)
        {
            if (backgroundLavaTilemap != null)
            {
                EnsureBackgroundTilemapSettings(backgroundLavaTilemap);
                tilemap = backgroundLavaTilemap;
                return true;
            }

            Tilemap[] childTilemaps = GetComponentsInChildren<Tilemap>(true);
            foreach (Tilemap childTilemap in childTilemaps)
            {
                if (childTilemap == null) continue;
                if (childTilemap.name.IndexOf("BackgroundLava", StringComparison.OrdinalIgnoreCase) < 0 &&
                    childTilemap.name.IndexOf("Lava", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                backgroundLavaTilemap = childTilemap;
                EnsureBackgroundTilemapSettings(backgroundLavaTilemap);
                tilemap = backgroundLavaTilemap;
                return true;
            }

            tilemap = null;
            return false;
        }

        private Tilemap GetOrCreateBackgroundWaterTilemap()
        {
            if (TryGetBackgroundWaterTilemap(out Tilemap existingTilemap))
            {
                return existingTilemap;
            }

            GameObject waterObject = new GameObject("BackgroundWater");
            waterObject.layer = gameObject.layer;
            waterObject.transform.SetParent(transform, false);

            backgroundWaterTilemap = waterObject.AddComponent<Tilemap>();
            waterObject.AddComponent<TilemapRenderer>();
            EnsureBackgroundTilemapSettings(backgroundWaterTilemap);
            return backgroundWaterTilemap;
        }

        private Tilemap GetOrCreateBackgroundLavaTilemap()
        {
            if (TryGetBackgroundLavaTilemap(out Tilemap existingTilemap))
            {
                return existingTilemap;
            }

            GameObject lavaObject = new GameObject("BackgroundLava");
            lavaObject.layer = gameObject.layer;
            lavaObject.transform.SetParent(transform, false);

            backgroundLavaTilemap = lavaObject.AddComponent<Tilemap>();
            lavaObject.AddComponent<TilemapRenderer>();
            EnsureBackgroundTilemapSettings(backgroundLavaTilemap);
            return backgroundLavaTilemap;
        }

        private void EnsureBackgroundWaterTilemapSettings(Tilemap tilemap)
        {
            EnsureBackgroundTilemapSettings(tilemap);
        }

        private void EnsureBackgroundTilemapSettings(Tilemap tilemap)
        {
            if (tilemap == null) return;

            TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer == null)
            {
                renderer = tilemap.gameObject.AddComponent<TilemapRenderer>();
            }

            renderer.sortingOrder = backgroundWaterSortingOrder;

            if (groundTilemap == null) return;

            TilemapRenderer groundRenderer = groundTilemap.GetComponent<TilemapRenderer>();
            if (groundRenderer == null) return;

            renderer.sortingLayerID = groundRenderer.sortingLayerID;
            renderer.sortingOrder = Mathf.Min(backgroundWaterSortingOrder, groundRenderer.sortingOrder - 1);
        }

        private static void GetLayoutHorizontalBounds(PlatformSegment[] layout, out int minX, out int maxX)
        {
            minX = int.MaxValue;
            maxX = int.MinValue;

            if (layout == null || layout.Length == 0)
            {
                minX = 0;
                maxX = 0;
                return;
            }

            for (int i = 0; i < layout.Length; i++)
            {
                PlatformSegment segment = layout[i];
                if (segment.xMin < minX) minX = segment.xMin;
                if (segment.xMax > maxX) maxX = segment.xMax;
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
            return selectedMap == MapTheme.BeginnerIsland ||
                   selectedMap == MapTheme.FoggyForest ||
                   selectedMap == MapTheme.VolcanoCave;
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
            else if (selectedMap == MapTheme.VolcanoCave)
            {
                treasureCellX = VolcanoCaveTreasureCellX;
                treasureSurfaceY = VolcanoCaveTreasureSurfaceY;
            }
        }

        private Vector2Int[] GetHealthPickupPlacementsForSelectedMap()
        {
            switch (selectedMap)
            {
                case MapTheme.FoggyForest:
                    return new[]
                    {
                        new Vector2Int(30, -4),
                        new Vector2Int(66, -4),
                    };
                case MapTheme.VolcanoCave:
                    return new[]
                    {
                        new Vector2Int(28, -4),
                        new Vector2Int(60, -4),
                        new Vector2Int(82, -3),
                    };
                default:
                    return new[]
                    {
                        new Vector2Int(30, -4),
                        new Vector2Int(86, -3),
                    };
            }
        }

        private ColoredPlacement[] GetTreasurePlacementsForSelectedMap()
        {
            switch (selectedMap)
            {
                case MapTheme.FoggyForest:
                    return FoggyForestTreasurePlacements;
                case MapTheme.VolcanoCave:
                    return VolcanoCaveTreasurePlacements;
                default:
                    return BeginnerTreasurePlacements;
            }
        }

        private ColoredPlacement[] GetKeyPlacementsForSelectedMap()
        {
            switch (selectedMap)
            {
                case MapTheme.FoggyForest:
                    return FoggyForestKeyPlacements;
                case MapTheme.VolcanoCave:
                    return VolcanoCaveKeyPlacements;
                default:
                    return BeginnerKeyPlacements;
            }
        }

        private Sprite GetKeyPickupSprite(TreasureKeyColor color)
        {
            switch (color)
            {
                case TreasureKeyColor.Red:
                    return redKeyPickupSprite != null ? redKeyPickupSprite : keyPickupSprite;
                case TreasureKeyColor.Green:
                    return greenKeyPickupSprite != null ? greenKeyPickupSprite : keyPickupSprite;
                case TreasureKeyColor.Blue:
                    return blueKeyPickupSprite != null ? blueKeyPickupSprite : keyPickupSprite;
                default:
                    return keyPickupSprite;
            }
        }

        private Sprite GetChestBodySprite(TreasureKeyColor color)
        {
            switch (color)
            {
                case TreasureKeyColor.Red:
                    return redChestBodySprite != null ? redChestBodySprite : yellowChestBodySprite;
                case TreasureKeyColor.Green:
                    return greenChestBodySprite != null ? greenChestBodySprite : yellowChestBodySprite;
                case TreasureKeyColor.Blue:
                    return blueChestBodySprite != null ? blueChestBodySprite : yellowChestBodySprite;
                default:
                    return yellowChestBodySprite;
            }
        }

        private Sprite GetChestLockSprite(TreasureKeyColor color)
        {
            switch (color)
            {
                case TreasureKeyColor.Red:
                    return redChestLockSprite != null ? redChestLockSprite : yellowChestLockSprite;
                case TreasureKeyColor.Green:
                    return greenChestLockSprite != null ? greenChestLockSprite : yellowChestLockSprite;
                case TreasureKeyColor.Blue:
                    return blueChestLockSprite != null ? blueChestLockSprite : yellowChestLockSprite;
                default:
                    return yellowChestLockSprite;
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
