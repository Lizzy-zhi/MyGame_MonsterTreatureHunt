using System.Collections;
using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class TreasureCollectible : MonoBehaviour
    {
        [SerializeField] private string collectorTag = "Player";
        [SerializeField] private bool disableOnCollect = true;
        [SerializeField] private bool requiresKey = true;
        [SerializeField] private TreasureKeyColor requiredKeyColor = TreasureKeyColor.Yellow;
        [SerializeField] private Vector2 triggerSize = new Vector2(1.35f, 1.25f);
        [SerializeField] private Vector2 triggerOffset = new Vector2(0f, 0.22f);
        [SerializeField] private Sprite unlockEffectSprite;
        [SerializeField] private int unlockEffectSortingOrder = 6;
        [SerializeField] private float unlockEffectDuration = 0.9f;
        [SerializeField] private float unlockEffectRadius = 1.15f;
        [SerializeField] private Vector3 unlockEffectOffset = new Vector3(0f, 0.45f, 0f);

        public bool IsCollected { get; private set; }
        public TreasureKeyColor RequiredKeyColor => requiredKeyColor;

        public delegate void CollectedHandler(TreasureCollectible treasure);
        public event CollectedHandler Collected;
        public event CollectedHandler Locked;
        public event CollectedHandler Unlocked;

        private bool isOpening;
        private Vector3 defaultScale;
        private Coroutine openRoutine;

        private void Awake()
        {
            defaultScale = transform.localScale;
            EnsureCollectTrigger();
        }

        private void Reset()
        {
            EnsureCollectTrigger();
        }

        private void EnsureCollectTrigger()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col == null) return;

            col.isTrigger = true;

            if (col is not BoxCollider2D box) return;

            box.size = triggerSize;
            box.offset = triggerOffset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryCollect(other);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryCollect(collision.collider);
        }

        private void TryCollect(Collider2D other)
        {
            if (IsCollected || isOpening || other == null || !other.CompareTag(collectorTag)) return;

            if (requiresKey)
            {
                PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
                if (inventory == null || !inventory.TryConsumeKey(requiredKeyColor))
                {
                    Locked?.Invoke(this);
                    return;
                }
            }

            IsCollected = true;
            isOpening = true;
            Unlocked?.Invoke(this);

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            if (Application.isPlaying)
            {
                openRoutine = StartCoroutine(PlayUnlockEffectThenCollect());
            }
            else
            {
                CompleteCollection();
            }
        }

        public void ResetCollectible()
        {
            if (openRoutine != null)
            {
                StopCoroutine(openRoutine);
                openRoutine = null;
            }

            if (defaultScale == Vector3.zero)
            {
                defaultScale = transform.localScale;
            }

            gameObject.SetActive(true);
            IsCollected = false;
            isOpening = false;
            transform.localScale = defaultScale;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
                col.isTrigger = true;
            }

            EnsureCollectTrigger();
        }

        public void ConfigureKeyRequirement(TreasureKeyColor keyColor, bool requireKey = true)
        {
            requiredKeyColor = keyColor;
            requiresKey = requireKey;
        }

        public void ConfigureTrigger(Vector2 size, Vector2 offset)
        {
            triggerSize = size;
            triggerOffset = offset;
            EnsureCollectTrigger();
        }

        public void ConfigureUnlockEffect(Sprite effectSprite)
        {
            unlockEffectSprite = effectSprite;
        }

        private IEnumerator PlayUnlockEffectThenCollect()
        {
            Transform effectRoot = CreateUnlockEffectRoot(out SpriteRenderer[] stars, out Vector3[] directions);
            float elapsed = 0f;

            while (elapsed < unlockEffectDuration)
            {
                float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, unlockEffectDuration));
                float pop = 1f + Mathf.Sin(t * Mathf.PI) * 0.18f;
                transform.localScale = defaultScale * pop;

                AnimateStars(effectRoot, stars, directions, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = defaultScale;
            if (effectRoot != null)
            {
                Destroy(effectRoot.gameObject);
            }

            CompleteCollection();
        }

        private Transform CreateUnlockEffectRoot(out SpriteRenderer[] stars, out Vector3[] directions)
        {
            const int StarCount = 6;
            stars = new SpriteRenderer[StarCount];
            directions = new Vector3[StarCount];

            GameObject rootObject = new GameObject("ChestUnlockEffect");
            Transform root = rootObject.transform;
            root.SetParent(transform, true);
            root.position = transform.position + unlockEffectOffset;

            if (unlockEffectSprite == null) return root;

            for (int i = 0; i < StarCount; i++)
            {
                float angle = 360f / StarCount * i + 20f;
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
                directions[i] = direction;

                GameObject starObject = new GameObject($"UnlockStar_{i + 1:00}");
                Transform starTransform = starObject.transform;
                starTransform.SetParent(root, false);
                starTransform.localPosition = Vector3.zero;
                starTransform.localScale = Vector3.one * 0.55f;

                SpriteRenderer star = starObject.AddComponent<SpriteRenderer>();
                star.sprite = unlockEffectSprite;
                star.sortingOrder = unlockEffectSortingOrder;
                stars[i] = star;
            }

            return root;
        }

        private void AnimateStars(Transform effectRoot, SpriteRenderer[] stars, Vector3[] directions, float t)
        {
            if (effectRoot == null || stars == null || directions == null) return;

            for (int i = 0; i < stars.Length; i++)
            {
                SpriteRenderer star = stars[i];
                if (star == null) continue;

                Transform starTransform = star.transform;
                starTransform.localPosition = directions[i] * (unlockEffectRadius * t);
                starTransform.localScale = Vector3.one * Mathf.Lerp(0.35f, 0.95f, t);
                starTransform.localRotation = Quaternion.Euler(0f, 0f, 220f * t);
                star.color = new Color(1f, 1f, 1f, 1f - t);
            }
        }

        private void CompleteCollection()
        {
            isOpening = false;
            openRoutine = null;
            Collected?.Invoke(this);

            if (disableOnCollect)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
