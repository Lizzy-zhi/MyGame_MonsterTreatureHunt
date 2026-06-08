using UnityEngine;
using MonsterTreasureHunt.Gameplay;

namespace MonsterTreasureHunt.Levels
{
    public class IslandLevelController : MonoBehaviour
    {
        [SerializeField] private TreasureCollectible treasure;

        public bool IsCompleted { get; private set; }

        public delegate void LevelCompletedHandler();
        public event LevelCompletedHandler LevelCompleted;

        private void OnEnable()
        {
            EnsureTreasureAssigned();

            if (treasure != null)
            {
                treasure.Collected += HandleTreasureCollected;
            }
        }

        private void OnDisable()
        {
            if (treasure != null)
            {
                treasure.Collected -= HandleTreasureCollected;
            }
        }

        public void ResetLevel()
        {
            IsCompleted = false;
            EnsureTreasureAssigned();

            if (treasure != null)
            {
                treasure.ResetCollectible();
            }
        }

        private void HandleTreasureCollected(TreasureCollectible collectedTreasure)
        {
            if (IsCompleted) return;

            IsCompleted = true;
            LevelCompleted?.Invoke();
        }

        private void EnsureTreasureAssigned()
        {
            if (treasure == null)
            {
                treasure = FindObjectOfType<TreasureCollectible>();
            }
        }
    }
}
