using UnityEngine;
using MonsterTreasureHunt.Gameplay;
using System;

namespace MonsterTreasureHunt.Levels
{
    public class LevelCompletionController : MonoBehaviour
    {
        [SerializeField] private TreasureCollectible treasure;
        [SerializeField] private TreasureCollectible[] treasures = Array.Empty<TreasureCollectible>();

        public bool IsCompleted { get; private set; }

        public delegate void LevelCompletedHandler();
        public event LevelCompletedHandler LevelCompleted;

        private void OnEnable()
        {
            RegisterTreasureCallbacks();
        }

        private void OnDisable()
        {
            UnregisterTreasureCallbacks();
        }

        public void ResetLevel()
        {
            IsCompleted = false;
            RegisterTreasureCallbacks();

            for (int i = 0; i < treasures.Length; i++)
            {
                if (treasures[i] != null)
                {
                    treasures[i].ResetCollectible();
                }
            }
        }

        private void HandleTreasureCollected(TreasureCollectible collectedTreasure)
        {
            if (IsCompleted) return;
            if (!AllTreasuresCollected()) return;

            IsCompleted = true;
            LevelCompleted?.Invoke();
        }

        private void RegisterTreasureCallbacks()
        {
            UnregisterTreasureCallbacks();

            treasures = FindObjectsOfType<TreasureCollectible>(false);
            if ((treasures == null || treasures.Length == 0) && treasure != null)
            {
                treasures = new[] { treasure };
            }

            if (treasures == null)
            {
                treasures = Array.Empty<TreasureCollectible>();
            }

            for (int i = 0; i < treasures.Length; i++)
            {
                if (treasures[i] != null)
                {
                    treasures[i].Collected += HandleTreasureCollected;
                }
            }
        }

        private void UnregisterTreasureCallbacks()
        {
            if (treasures == null) return;

            for (int i = 0; i < treasures.Length; i++)
            {
                if (treasures[i] != null)
                {
                    treasures[i].Collected -= HandleTreasureCollected;
                }
            }
        }

        private bool AllTreasuresCollected()
        {
            if (treasures == null || treasures.Length == 0) return false;

            for (int i = 0; i < treasures.Length; i++)
            {
                if (treasures[i] != null && !treasures[i].IsCollected)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
