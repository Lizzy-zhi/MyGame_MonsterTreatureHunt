using System.Collections;
using MonsterTreasureHunt.Player;
using MonsterTreasureHunt.UI;
using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class FakeTreasureChestTrap : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private Sprite disguisedChestSprite;
        [SerializeField] private Sprite monsterRestSprite;
        [SerializeField] private Sprite monsterAlertSpriteA;
        [SerializeField] private Sprite monsterAlertSpriteB;
        [SerializeField] private Sprite explosionSprite;
        [SerializeField] private float countdownDuration = 3f;
        [SerializeField] private float explosionRadius = 2.1f;
        [SerializeField] private int explosionDamage = 3;
        [SerializeField] private float alertAnimationRate = 7f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private Vector2 triggerSize = new Vector2(0.78f, 0.72f);
        [SerializeField] private Vector2 triggerOffset = new Vector2(0f, 0.05f);
        [SerializeField] private string hintText = "Fake chest! Move away before it explodes.";

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D triggerCollider;
        private Vector3 defaultScale;
        private Coroutine trapRoutine;
        private bool triggered;
        private HUDManager hudManager;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            triggerCollider = GetComponent<BoxCollider2D>();
            defaultScale = transform.localScale;
            ApplyTriggerShape();
            ShowChestForm();
        }

        private void OnEnable()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (triggerCollider == null) triggerCollider = GetComponent<BoxCollider2D>();
            if (defaultScale == Vector3.zero) defaultScale = transform.localScale;

            triggered = false;
            transform.localScale = defaultScale;
            ApplyTriggerShape();
            ShowChestForm();
        }

        private void Reset()
        {
            triggerCollider = GetComponent<BoxCollider2D>();
            ApplyTriggerShape();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered || other == null || !other.CompareTag(playerTag)) return;

            triggered = true;
            hudManager ??= FindObjectOfType<HUDManager>();
            hudManager?.ShowGameplayHint(hintText, countdownDuration + 0.35f);

            if (trapRoutine != null)
            {
                StopCoroutine(trapRoutine);
            }

            trapRoutine = StartCoroutine(ActivateTrap());
        }

        public void ConfigureSprites(Sprite chestSprite, Sprite restSprite, Sprite alertSpriteA, Sprite alertSpriteB, Sprite blastSprite)
        {
            disguisedChestSprite = chestSprite;
            monsterRestSprite = restSprite;
            monsterAlertSpriteA = alertSpriteA;
            monsterAlertSpriteB = alertSpriteB;
            explosionSprite = blastSprite;
            ShowChestForm();
        }

        public void ConfigureExplosion(float countdown, float radius, int damage)
        {
            countdownDuration = Mathf.Max(0.2f, countdown);
            explosionRadius = Mathf.Max(0.3f, radius);
            explosionDamage = Mathf.Max(1, damage);
        }

        public void ConfigureTrigger(Vector2 size, Vector2 offset)
        {
            triggerSize = size;
            triggerOffset = offset;
            ApplyTriggerShape();
        }

        private void ApplyTriggerShape()
        {
            if (triggerCollider == null) return;

            triggerCollider.isTrigger = true;
            triggerCollider.enabled = true;
            triggerCollider.size = triggerSize;
            triggerCollider.offset = triggerOffset;
        }

        private IEnumerator ActivateTrap()
        {
            float elapsed = 0f;

            while (elapsed < countdownDuration)
            {
                float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, countdownDuration));
                UpdateMonsterForm(t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            Explode();
        }

        private void UpdateMonsterForm(float t)
        {
            if (spriteRenderer == null) return;

            Sprite currentSprite = monsterRestSprite;
            if (monsterAlertSpriteA != null || monsterAlertSpriteB != null)
            {
                int frame = Mathf.FloorToInt(Time.time * alertAnimationRate) % 2;
                currentSprite = frame == 0
                    ? (monsterAlertSpriteA != null ? monsterAlertSpriteA : monsterRestSprite)
                    : (monsterAlertSpriteB != null ? monsterAlertSpriteB : monsterAlertSpriteA);
            }

            if (currentSprite != null)
            {
                spriteRenderer.sprite = currentSprite;
            }

            spriteRenderer.color = Color.Lerp(Color.white, new Color(1f, 0.55f, 0.35f, 1f), t);
            float pulse = 1f + Mathf.Sin(Time.time * 16f) * Mathf.Lerp(0.03f, 0.12f, t);
            transform.localScale = defaultScale * Mathf.Lerp(1f, pulseScale, t) * pulse;
        }

        private void Explode()
        {
            trapRoutine = null;

            DamageNearbyPlayer();

            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            if (spriteRenderer != null)
            {
                if (explosionSprite != null)
                {
                    spriteRenderer.sprite = explosionSprite;
                }

                spriteRenderer.color = new Color(1f, 0.8f, 0.35f, 1f);
            }

            transform.localScale = defaultScale * 1.55f;
            StartCoroutine(HideAfterBlast());
        }

        private void DamageNearbyPlayer()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            for (int i = 0; i < hits.Length; i++)
            {
                Collider2D hit = hits[i];
                if (hit == null || !hit.CompareTag(playerTag)) continue;

                PlayerHealth health = hit.GetComponentInParent<PlayerHealth>();
                if (health == null || health.IsDepleted) continue;

                if (!health.Damage(explosionDamage, PlayerHealth.DamageSource.FakeChestExplosion))
                {
                    continue;
                }

                PlayerMovement movement = hit.GetComponentInParent<PlayerMovement>();
                movement?.PlayHurtFeedback(hit.transform.position.x - transform.position.x);
                break;
            }
        }

        private IEnumerator HideAfterBlast()
        {
            yield return new WaitForSeconds(0.35f);
            gameObject.SetActive(false);
        }

        private void ShowChestForm()
        {
            if (spriteRenderer == null) return;

            if (disguisedChestSprite != null && !triggered)
            {
                spriteRenderer.sprite = disguisedChestSprite;
            }

            spriteRenderer.color = Color.white;
        }
    }
}
