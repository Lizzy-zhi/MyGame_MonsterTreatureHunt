using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class TreasureCollectible : MonoBehaviour
    {
        [SerializeField] private string collectorTag = "Player";
        [SerializeField] private bool disableOnCollect = true;

        public bool IsCollected { get; private set; }

        public delegate void CollectedHandler(TreasureCollectible treasure);
        public event CollectedHandler Collected;

        private void Awake()
        {
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

            if (box.size.x >= 0.01f && box.size.y >= 0.01f) return;

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null && sprite.sprite != null)
            {
                box.size = sprite.sprite.bounds.size;
            }
            else
            {
                box.size = new Vector2(0.64f, 0.64f);
            }
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
            if (IsCollected || other == null || !other.CompareTag(collectorTag)) return;

            IsCollected = true;
            Collected?.Invoke(this);

            if (disableOnCollect)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
