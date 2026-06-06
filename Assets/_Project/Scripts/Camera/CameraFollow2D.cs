using System;
using UnityEngine;

namespace MonsterTreasureHunt.CameraSystem
{
    public class CameraFollow2D : MonoBehaviour
    {
        private const int ParallaxTileCount = 7;
        private const float TileOverlapRatio = 0.02f;
        private const float BaseLayerDepth = 18f;
        private const float SortingDepthSpacing = 0.01f;

        [System.Serializable]
        private class ParallaxLayer
        {
            public string name = "ParallaxLayer";
            public Sprite sprite = null;
            [Range(0f, 1f)] public float parallaxFactor = 0.25f;
            public Vector2 offset = Vector2.zero;
            public float scale = 1f;
            public int sortingOrder = -20;
            [Range(0f, 1f)] public float alpha = 1f;
            [NonSerialized] public Transform[] tiles;
            [NonSerialized] public float spriteWorldWidth;
            [NonSerialized] public float tileSpacing;
        }

        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime = 0.2f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 1.2f, -10f);
        [SerializeField] private bool buildParallaxBackground = true;
        [SerializeField, Range(1f, 2f)] private float parallaxOverscan = 1.25f;
        [SerializeField] private ParallaxLayer[] parallaxLayers;

        private Camera attachedCamera;
        private Vector3 parallaxOriginPosition;
        private Vector3 velocity;

        private void Awake()
        {
            attachedCamera = GetComponent<Camera>();
            parallaxOriginPosition = transform.position;

            if (buildParallaxBackground)
            {
                CreateParallaxLayers();
            }
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }

            UpdateParallaxLayers();
        }

        private void CreateParallaxLayers()
        {
            if (parallaxLayers == null) return;

            for (int i = 0; i < parallaxLayers.Length; i++)
            {
                ParallaxLayer layer = parallaxLayers[i];
                if (layer == null || layer.sprite == null) continue;

                layer.tiles = new Transform[ParallaxTileCount];

                for (int tileIndex = 0; tileIndex < layer.tiles.Length; tileIndex++)
                {
                    string tileName = $"{layer.name}_{tileIndex}";
                    Transform existing = transform.Find(tileName);
                    GameObject layerObject = existing != null ? existing.gameObject : new GameObject(tileName);
                    layer.tiles[tileIndex] = layerObject.transform;
                    layer.tiles[tileIndex].SetParent(transform, false);

                    SpriteRenderer renderer = layerObject.GetComponent<SpriteRenderer>();
                    if (renderer == null)
                    {
                        renderer = layerObject.AddComponent<SpriteRenderer>();
                    }

                    renderer.sprite = layer.sprite;
                    renderer.sortingOrder = layer.sortingOrder;
                    renderer.color = new Color(1f, 1f, 1f, layer.alpha);
                }

                layer.spriteWorldWidth = Mathf.Max(0.01f, layer.sprite.bounds.size.x * layer.scale);
                layer.tileSpacing = layer.spriteWorldWidth;
            }

            UpdateParallaxLayers();
        }

        private void UpdateParallaxLayers()
        {
            if (!buildParallaxBackground || parallaxLayers == null) return;

            foreach (ParallaxLayer layer in parallaxLayers)
            {
                if (layer == null || layer.tiles == null || layer.sprite == null) continue;

                float layerZ = GetLayerZ(layer);
                float cameraHeight = GetVisibleHeight(layerZ);
                float visibleWidth = GetVisibleWidth(cameraHeight);
                ScaleLayerToCamera(layer, cameraHeight, visibleWidth);

                float cameraX = transform.position.x;
                float cameraLeft = cameraX - visibleWidth * 0.5f;
                float tileSpacing = Mathf.Max(0.01f, layer.tileSpacing);
                float movementFromOrigin = cameraX - parallaxOriginPosition.x;
                float centerX = cameraX - movementFromOrigin * layer.parallaxFactor + layer.offset.x;
                float startX = centerX - tileSpacing * (layer.tiles.Length / 2);
                while (startX > cameraLeft - tileSpacing)
                {
                    startX -= tileSpacing;
                }

                while (startX + tileSpacing < cameraLeft - tileSpacing)
                {
                    startX += tileSpacing;
                }

                for (int i = 0; i < layer.tiles.Length; i++)
                {
                    if (layer.tiles[i] == null) continue;

                    layer.tiles[i].position = new Vector3(
                        startX + tileSpacing * i,
                        transform.position.y + layer.offset.y,
                        layerZ);
                }
            }
        }

        private float GetLayerZ(ParallaxLayer layer)
        {
            return transform.position.z + BaseLayerDepth + layer.sortingOrder * SortingDepthSpacing;
        }

        private float GetVisibleHeight(float layerZ)
        {
            if (attachedCamera == null)
            {
                return 10f;
            }

            if (attachedCamera.orthographic)
            {
                return attachedCamera.orthographicSize * 2f;
            }

            float distance = Mathf.Abs(layerZ - transform.position.z);
            return 2f * distance * Mathf.Tan(attachedCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        private float GetVisibleWidth(float visibleHeight)
        {
            return attachedCamera != null ? visibleHeight * attachedCamera.aspect : visibleHeight * 1.78f;
        }

        private void ScaleLayerToCamera(ParallaxLayer layer, float visibleHeight, float visibleWidth)
        {
            Vector2 spriteSize = layer.sprite.bounds.size;
            if (spriteSize.x <= 0f || spriteSize.y <= 0f) return;

            float overscan = Mathf.Max(1f, parallaxOverscan);
            float scaleToCover = Mathf.Max(
                visibleWidth * overscan / spriteSize.x,
                visibleHeight * overscan / spriteSize.y);
            float finalScale = scaleToCover * layer.scale;
            layer.spriteWorldWidth = spriteSize.x * finalScale;
            layer.tileSpacing = layer.spriteWorldWidth * (1f - TileOverlapRatio);

            if (layer.tiles == null) return;

            foreach (Transform tile in layer.tiles)
            {
                if (tile != null)
                {
                    tile.localScale = new Vector3(finalScale, finalScale, 1f);
                }
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
