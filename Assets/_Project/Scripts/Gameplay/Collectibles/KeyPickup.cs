using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class KeyPickup : MonoBehaviour
    {
        [SerializeField] private string collectorTag = "Player";
        [SerializeField] private TreasureKeyColor keyColor = TreasureKeyColor.Yellow;
        [SerializeField] private int keyAmount = 1;

        private bool collected;

        public TreasureKeyColor KeyColor
        {
            get => keyColor;
            set => keyColor = value;
        }

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
            if (collected || other == null || !other.CompareTag(collectorTag)) return;

            PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
            if (inventory == null) return;

            inventory.AddKey(keyColor, keyAmount);
            collected = true;
            gameObject.SetActive(false);
        }
    }
}
