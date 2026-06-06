using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] private string collectorTag = "Player";
        [SerializeField] private int healAmount = 1;
        [SerializeField] private bool disableOnCollect = true;

        private bool collected;

        private void Awake()
        {
            EnsurePickupTrigger();
        }

        private void Reset()
        {
            EnsurePickupTrigger();
        }

        private void EnsurePickupTrigger()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col == null) return;

            col.isTrigger = true;

            if (col is BoxCollider2D existingBox && existingBox.size.x >= 0.1f && existingBox.size.y >= 0.1f)
            {
                return;
            }

            if (col is CircleCollider2D existingCircle && existingCircle.radius >= 0.1f)
            {
                return;
            }

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            Vector2 pickupSize = new Vector2(0.64f, 0.64f);
            if (sprite != null && sprite.sprite != null)
            {
                pickupSize = sprite.sprite.bounds.size * 0.84f;
            }

            if (col is CircleCollider2D circle)
            {
                float radius = Mathf.Max(pickupSize.x, pickupSize.y) * 0.5f;
                circle.radius = Mathf.Max(0.1f, radius);
            }
            else if (col is BoxCollider2D box)
            {
                box.size = new Vector2(Mathf.Max(0.1f, pickupSize.x), Mathf.Max(0.1f, pickupSize.y));
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
            if (collected || other == null || !other.CompareTag(collectorTag)) return;

            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health == null) return;

            health.Heal(healAmount);
            collected = true;
            if (disableOnCollect)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
