using UnityEngine;
using MonsterTreasureHunt.Gameplay;

namespace MonsterTreasureHunt.Levels
{
    public class BeginnerIslandLevelController : MonoBehaviour
    {
        [SerializeField] private TreasureCollectible treasure;

        public bool IsCompleted { get; private set; }

        public delegate void LevelCompletedHandler();
        public event LevelCompletedHandler LevelCompleted;

        private void OnEnable()
        {
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

        private void HandleTreasureCollected(TreasureCollectible collectedTreasure)
        {
            if (IsCompleted) return;

            IsCompleted = true;
            LevelCompleted?.Invoke();
        }
    }
}
