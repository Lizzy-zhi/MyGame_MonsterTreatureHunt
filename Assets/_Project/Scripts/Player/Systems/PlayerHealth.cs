using System;
using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [DisallowMultipleComponent]
    public class PlayerHealth : MonoBehaviour
    {
        public enum DamageSource
        {
            Unknown = 0,
            Fall = 1,
            Spike = 2,
            Bee = 3,
            FireSlime = 4,
            FakeChestExplosion = 5,
            Fish = 6,
        }

        [SerializeField] private int maxLives = 4;
        [SerializeField] private int currentLives = 4;

        public int MaxLives => maxLives;
        public int CurrentLives => currentLives;
        public bool IsDepleted => currentLives <= 0;
        public bool IsFull => currentLives >= maxLives;
        public DamageSource LastDamageSource { get; private set; }

        public delegate void HealthChangedHandler(int currentLives, int maxLives);
        public event HealthChangedHandler HealthChanged;

        public delegate void DamagedHandler(DamageSource source);
        public event DamagedHandler Damaged;

        public void ResetHealth(int lives)
        {
            maxLives = Mathf.Max(1, lives);
            currentLives = maxLives;
            HealthChanged?.Invoke(currentLives, maxLives);
        }

        public bool Damage(int amount, DamageSource source = DamageSource.Unknown)
        {
            if (amount <= 0 || currentLives <= 0) return false;

            int previousLives = currentLives;
            currentLives = Mathf.Max(0, currentLives - amount);
            if (currentLives == previousLives) return false;

            LastDamageSource = source;
            Damaged?.Invoke(source);
            HealthChanged?.Invoke(currentLives, maxLives);
            return true;
        }

        public bool Heal(int amount)
        {
            if (amount <= 0 || currentLives >= maxLives) return false;

            int previousLives = currentLives;
            currentLives = Mathf.Min(maxLives, currentLives + amount);
            if (currentLives == previousLives) return false;

            HealthChanged?.Invoke(currentLives, maxLives);
            return true;
        }
    }
}
