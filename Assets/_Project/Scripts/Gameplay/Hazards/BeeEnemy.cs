using UnityEngine;
using MonsterTreasureHunt.Player;
using MonsterTreasureHunt.UI;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class BeeEnemy : MonoBehaviour
    {
        [SerializeField] private string damageTargetTag = "Player";
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float damageCooldown = 1.1f;
        [SerializeField] private float moveSpeed = 1.9f;
        [SerializeField] private float patrolDistance = 2.6f;
        [SerializeField] private float bobAmplitude = 0.12f;
        [SerializeField] private float bobFrequency = 2.2f;
        [SerializeField] private Sprite restSprite;
        [SerializeField] private Sprite flapSpriteA;
        [SerializeField] private Sprite flapSpriteB;
        [SerializeField] private float flapRate = 11f;
        [SerializeField] private float hintHorizontalRange = 7.5f;
        [SerializeField] private float hintVerticalRange = 2f;
        [SerializeField] private float hintForwardBuffer = 1.2f;
        [SerializeField] private string crouchHintText = "Only crouching lets you dodge the bee.";

        private SpriteRenderer spriteRenderer;
        private Vector3 startPosition;
        private float nextDamageTime;
        private float flapTimer;
        private int direction = 1;
        private HUDManager hudManager;
        private bool hintVisible;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPosition = transform.position;
            hudManager = FindObjectOfType<HUDManager>();

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
            flapTimer = 0f;
            direction = 1;
            hintVisible = false;
        }

        private void Update()
        {
            UpdateMovement();
            UpdateVisual();
            UpdateHint();
        }

        private void OnDisable()
        {
            HideHint();
        }

        private void OnDestroy()
        {
            HideHint();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDamage(other);
        }

        public void ConfigureSprites(Sprite rest, Sprite flapA, Sprite flapB)
        {
            restSprite = rest;
            flapSpriteA = flapA;
            flapSpriteB = flapB;
            if (spriteRenderer != null && restSprite != null)
            {
                spriteRenderer.sprite = restSprite;
            }
        }

        public void ConfigureMovement(float speed, float horizontalPatrolDistance, float verticalBobAmplitude)
        {
            moveSpeed = Mathf.Max(0.05f, speed);
            patrolDistance = Mathf.Max(0.25f, horizontalPatrolDistance);
            bobAmplitude = Mathf.Max(0f, verticalBobAmplitude);
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

            float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.position = new Vector3(nextX, startPosition.y + bobOffset, startPosition.z);
        }

        private void UpdateVisual()
        {
            if (spriteRenderer == null) return;

            spriteRenderer.flipX = direction < 0;

            if (flapSpriteA == null && flapSpriteB == null)
            {
                if (restSprite != null) spriteRenderer.sprite = restSprite;
                return;
            }

            flapTimer += Time.deltaTime * flapRate;
            int frame = Mathf.FloorToInt(flapTimer) % 2;
            Sprite flapSprite = frame == 0 ? flapSpriteA : flapSpriteB;

            if (flapSprite != null)
            {
                spriteRenderer.sprite = flapSprite;
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

            if (!health.Damage(damageAmount, PlayerHealth.DamageSource.Bee)) return;

            float knockbackDirectionX = other.transform.position.x - transform.position.x;
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement?.PlayHurtFeedback(knockbackDirectionX);

            nextDamageTime = Time.time + damageCooldown;
        }

        private void UpdateHint()
        {
            if (hudManager == null)
            {
                hudManager = FindObjectOfType<HUDManager>();
                if (hudManager == null) return;
            }

            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player == null || !player.enabled)
            {
                HideHint();
                return;
            }

            Vector2 playerOffset = player.transform.position - transform.position;
            bool isWithinHintWidth = playerOffset.x >= -hintHorizontalRange && playerOffset.x <= hintForwardBuffer;
            bool isWithinHintHeight = Mathf.Abs(playerOffset.y) <= hintVerticalRange;
            bool shouldShowHint = isWithinHintWidth && isWithinHintHeight && !player.IsCrouching;

            if (shouldShowHint)
            {
                hudManager.ShowGameplayHint(crouchHintText);
                hintVisible = true;
            }
            else
            {
                HideHint();
            }
        }

        private void HideHint()
        {
            if (!hintVisible) return;

            hintVisible = false;
            if (hudManager != null)
            {
                hudManager.HideGameplayHint(crouchHintText);
            }
        }
    }
}
