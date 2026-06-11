using UnityEngine;
using MonsterTreasureHunt.Player;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class FishEnemy : MonoBehaviour
    {
        [SerializeField] private string damageTargetTag = "Player";
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float damageCooldown = 1.1f;
        [SerializeField] private float jumpHeight = 3.2f;
        [SerializeField] private float jumpDuration = 1.05f;
        [SerializeField] private float restDuration = 1.35f;
        [SerializeField] private float hiddenDepth = 0.9f;
        [SerializeField] private float startDelay;
        [SerializeField] private float editorPreviewHeight = 0.55f;
        [SerializeField] private Sprite restSprite;
        [SerializeField] private Sprite jumpSprite;
        [SerializeField] private Sprite fallSprite;

        private SpriteRenderer spriteRenderer;
        private Collider2D damageCollider;
        private Vector3 basePosition;
        private float waterSurfaceY;
        private float cycleStartTime;
        private float nextDamageTime;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            damageCollider = GetComponent<Collider2D>();

            if (damageCollider != null)
            {
                damageCollider.isTrigger = true;
            }

            waterSurfaceY = transform.position.y;
            basePosition = new Vector3(transform.position.x, waterSurfaceY - hiddenDepth, transform.position.z);
            HideBelowWater();
        }

        private void OnEnable()
        {
            ResetCycleTimer();
            nextDamageTime = 0f;
        }

        private void Update()
        {
            UpdateJumpCycle();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDamage(other);
        }

        public void ConfigureSprites(Sprite rest, Sprite jump, Sprite fall)
        {
            restSprite = rest;
            jumpSprite = jump;
            fallSprite = fall;
        }

        public void ConfigureJump(float waterSurfaceWorldY, float hideDepth, float height, float activeDuration, float rest, float delay)
        {
            waterSurfaceY = waterSurfaceWorldY;
            hiddenDepth = Mathf.Max(0.1f, hideDepth);
            jumpHeight = Mathf.Max(0.1f, height);
            jumpDuration = Mathf.Max(0.2f, activeDuration);
            restDuration = Mathf.Max(0.1f, rest);
            startDelay = Mathf.Max(0f, delay);
            basePosition = new Vector3(transform.position.x, waterSurfaceY - hiddenDepth, transform.position.z);
            ResetCycleTimer();

            if (Application.isPlaying)
            {
                HideBelowWater();
            }
            else
            {
                ShowEditorPreview();
            }
        }

        private void ResetCycleTimer()
        {
            // startDelay means "time before the next jump", so offset past the hidden-rest phase.
            cycleStartTime = Time.time + startDelay - restDuration;
        }

        private void UpdateJumpCycle()
        {
            if (Time.time < cycleStartTime)
            {
                HideBelowWater();
                return;
            }

            float cycleLength = restDuration + jumpDuration;
            if (cycleLength <= Mathf.Epsilon)
            {
                HideBelowWater();
                return;
            }

            float elapsed = Mathf.Repeat(Time.time - cycleStartTime, cycleLength);
            if (elapsed < restDuration)
            {
                HideBelowWater();
                return;
            }

            float t = Mathf.Clamp01((elapsed - restDuration) / jumpDuration);
            float arc = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = new Vector3(basePosition.x, basePosition.y + arc, basePosition.z);

            bool isAboveWater = arc > hiddenDepth * 0.35f;
            SetActiveVisual(isAboveWater);

            if (spriteRenderer == null) return;

            Sprite activeSprite = t < 0.5f ? jumpSprite : fallSprite;
            if (activeSprite != null)
            {
                spriteRenderer.sprite = activeSprite;
            }
            else if (restSprite != null)
            {
                spriteRenderer.sprite = restSprite;
            }
        }

        private void HideBelowWater()
        {
            transform.position = basePosition;
            SetActiveVisual(false);

            if (spriteRenderer != null && restSprite != null)
            {
                spriteRenderer.sprite = restSprite;
            }
        }

        private void ShowEditorPreview()
        {
            transform.position = new Vector3(basePosition.x, waterSurfaceY + editorPreviewHeight, basePosition.z);

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = jumpSprite != null ? jumpSprite : restSprite;
            }

            if (damageCollider != null)
            {
                damageCollider.enabled = false;
            }
        }

        private void SetActiveVisual(bool active)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = active;
            }

            if (damageCollider != null)
            {
                damageCollider.enabled = active;
            }
        }

        private void TryDamage(Collider2D other)
        {
            if (damageCollider != null && !damageCollider.enabled) return;
            if (Time.time < nextDamageTime) return;
            if (other == null || !other.CompareTag(damageTargetTag)) return;

            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health == null || health.IsDepleted) return;

            if (!health.Damage(damageAmount, PlayerHealth.DamageSource.Fish)) return;

            float knockbackDirectionX = other.transform.position.x - transform.position.x;
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement?.PlayHurtFeedback(knockbackDirectionX);

            nextDamageTime = Time.time + damageCooldown;
        }
    }
}
