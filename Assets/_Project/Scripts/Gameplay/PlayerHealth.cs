using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [DisallowMultipleComponent]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxLives = 3;
        [SerializeField] private int currentLives = 3;

        public int MaxLives => maxLives;
        public int CurrentLives => currentLives;
        public bool IsDepleted => currentLives <= 0;
        public bool IsFull => currentLives >= maxLives;

        public delegate void HealthChangedHandler(int currentLives, int maxLives);
        public event HealthChangedHandler HealthChanged;

        public void ResetHealth(int lives)
        {
            maxLives = Mathf.Max(1, lives);
            currentLives = maxLives;
            HealthChanged?.Invoke(currentLives, maxLives);
        }

        public bool Damage(int amount)
        {
            if (amount <= 0 || currentLives <= 0) return false;

            int previousLives = currentLives;
            currentLives = Mathf.Max(0, currentLives - amount);
            if (currentLives == previousLives) return false;

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
