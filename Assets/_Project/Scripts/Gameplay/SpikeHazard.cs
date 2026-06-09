using UnityEngine;
using MonsterTreasureHunt.Player;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikeHazard : MonoBehaviour
    {
        [SerializeField] private string damageTargetTag = "Player";
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float damageCooldown = 1.25f;

        private float nextDamageTime;

        private void Awake()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void TryDamage(Collider2D other)
        {
            if (Time.time < nextDamageTime) return;
            if (other == null || !other.CompareTag(damageTargetTag)) return;

            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health == null || health.IsDepleted) return;

            if (!health.Damage(damageAmount, PlayerHealth.DamageSource.Hazard)) return;

            float knockbackDirectionX = other.transform.position.x - transform.position.x;
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement?.PlayHurtFeedback(knockbackDirectionX);

            nextDamageTime = Time.time + damageCooldown;
        }
    }

}
