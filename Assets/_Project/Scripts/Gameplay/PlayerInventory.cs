using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [DisallowMultipleComponent]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int yellowKeys;

        public int YellowKeys => yellowKeys;

        public delegate void InventoryChangedHandler(int yellowKeys);
        public event InventoryChangedHandler InventoryChanged;

        public void ResetInventory()
        {
            yellowKeys = 0;
            NotifyChanged();
        }

        public void AddYellowKey(int amount = 1)
        {
            if (amount <= 0) return;

            yellowKeys += amount;
            NotifyChanged();
        }

        public bool TryConsumeYellowKey()
        {
            if (yellowKeys <= 0) return false;

            yellowKeys--;
            NotifyChanged();
            return true;
        }

        private void NotifyChanged()
        {
            InventoryChanged?.Invoke(yellowKeys);
        }
    }
}
