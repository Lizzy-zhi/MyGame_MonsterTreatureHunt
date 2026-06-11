using UnityEngine;
using MonsterTreasureHunt.Player;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class FireSlimeEnemy : MonoBehaviour
    {
        [SerializeField] private string damageTargetTag = "Player";
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float damageCooldown = 1.1f;
        [SerializeField] private float moveSpeed = 1.45f;
        [SerializeField] private float patrolDistance = 2f;
        [SerializeField] private Sprite restSprite;
        [SerializeField] private Sprite walkSpriteA;
        [SerializeField] private Sprite walkSpriteB;
        [SerializeField] private float animationRate = 8.5f;

        private SpriteRenderer spriteRenderer;
        private Vector3 startPosition;
        private float nextDamageTime;
        private float animationTimer;
        private int direction = 1;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPosition = transform.position;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void OnEnable()
        {
            startPosition = transform.position;
            nextDamageTime = 0f;
            animationTimer = 0f;
            direction = 1;
        }

        private void Update()
        {
            UpdateMovement();
            UpdateVisual();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDamage(other);
        }

        public void ConfigureSprites(Sprite rest, Sprite walkA, Sprite walkB)
        {
            restSprite = rest;
            walkSpriteA = walkA;
            walkSpriteB = walkB;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = walkSpriteA != null ? walkSpriteA : restSprite;
            }
        }

        public void ConfigureMovement(float speed, float horizontalPatrolDistance)
        {
            moveSpeed = Mathf.Max(0.05f, speed);
            patrolDistance = Mathf.Max(0.25f, horizontalPatrolDistance);
        }

        private void UpdateMovement()
        {
            float nextX = transform.position.x + direction * moveSpeed * Time.deltaTime;
            float leftBound = startPosition.x - patrolDistance;
            float rightBound = startPosition.x + patrolDistance;

            if (nextX >= rightBound)
            {
                nextX = rightBound;
                direction = -1;
            }
            else if (nextX <= leftBound)
            {
                nextX = leftBound;
                direction = 1;
            }

            transform.position = new Vector3(nextX, startPosition.y, startPosition.z);
        }

        private void UpdateVisual()
        {
            if (spriteRenderer == null) return;

            spriteRenderer.flipX = direction < 0;

            if (walkSpriteA == null && walkSpriteB == null)
            {
                if (restSprite != null) spriteRenderer.sprite = restSprite;
                return;
            }

            animationTimer += Time.deltaTime * animationRate;
            int frame = Mathf.FloorToInt(animationTimer) % 2;
            Sprite walkSprite = frame == 0 ? walkSpriteA : walkSpriteB;

            if (walkSprite != null)
            {
                spriteRenderer.sprite = walkSprite;
            }
            else if (restSprite != null)
            {
                spriteRenderer.sprite = restSprite;
            }
        }

        private void TryDamage(Collider2D other)
        {
            if (Time.time < nextDamageTime) return;
            if (other == null || !other.CompareTag(damageTargetTag)) return;

            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health == null || health.IsDepleted) return;

            if (!health.Damage(damageAmount, PlayerHealth.DamageSource.FireSlime)) return;

            float knockbackDirectionX = other.transform.position.x - transform.position.x;
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement?.PlayHurtFeedback(knockbackDirectionX);

            nextDamageTime = Time.time + damageCooldown;
        }
    }
}
