using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [DisallowMultipleComponent]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int yellowKeys;
        [SerializeField] private int redKeys;
        [SerializeField] private int greenKeys;
        [SerializeField] private int blueKeys;

        public int YellowKeys => yellowKeys;
        public int RedKeys => redKeys;
        public int GreenKeys => greenKeys;
        public int BlueKeys => blueKeys;

        public delegate void InventoryChangedHandler(PlayerInventory inventory);
        public event InventoryChangedHandler InventoryChanged;

        public void ResetInventory()
        {
            yellowKeys = 0;
            redKeys = 0;
            greenKeys = 0;
            blueKeys = 0;
            NotifyChanged();
        }

        public void AddYellowKey(int amount = 1)
        {
            AddKey(TreasureKeyColor.Yellow, amount);
        }

        public bool TryConsumeYellowKey()
        {
            return TryConsumeKey(TreasureKeyColor.Yellow);
        }

        public int GetKeyCount(TreasureKeyColor color)
        {
            return color switch
            {
                TreasureKeyColor.Red => redKeys,
                TreasureKeyColor.Green => greenKeys,
                TreasureKeyColor.Blue => blueKeys,
                _ => yellowKeys
            };
        }

        public void AddKey(TreasureKeyColor color, int amount = 1)
        {
            if (amount <= 0) return;

            switch (color)
            {
                case TreasureKeyColor.Red:
                    redKeys += amount;
                    break;
                case TreasureKeyColor.Green:
                    greenKeys += amount;
                    break;
                case TreasureKeyColor.Blue:
                    blueKeys += amount;
                    break;
                default:
                    yellowKeys += amount;
                    break;
            }

            NotifyChanged();
        }

        public bool TryConsumeKey(TreasureKeyColor color)
        {
            if (GetKeyCount(color) <= 0) return false;

            switch (color)
            {
                case TreasureKeyColor.Red:
                    redKeys--;
                    break;
                case TreasureKeyColor.Green:
                    greenKeys--;
                    break;
                case TreasureKeyColor.Blue:
                    blueKeys--;
                    break;
                default:
                    yellowKeys--;
                    break;
            }

            NotifyChanged();
            return true;
        }

        private void NotifyChanged()
        {
            InventoryChanged?.Invoke(this);
        }
    }
}
