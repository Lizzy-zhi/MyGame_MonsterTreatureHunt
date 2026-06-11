using MonsterTreasureHunt.Player;
using UnityEngine;

namespace MonsterTreasureHunt.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LadderZone : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float snapOffsetX;
        [SerializeField] private bool hasTopExit;
        [SerializeField] private float topExitX;
        [SerializeField] private float topSurfaceY;
        [SerializeField] private float topPlatformCellHeight = 1f;

        public float SnapX => transform.position.x + snapOffsetX;
        public bool HasTopExit => hasTopExit;
        public float TopExitX => hasTopExit ? topExitX : SnapX;
        public float TopSurfaceY => topSurfaceY;
        public float TopPlatformBottomY => topSurfaceY - topPlatformCellHeight;

        private BoxCollider2D triggerCollider;

        private void Awake()
        {
            triggerCollider = GetComponent<BoxCollider2D>();
            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = true;
            }
        }

        public void ConfigureTopExit(float snapWorldX, float topExitWorldX, float topSurfaceWorldY, float platformCellHeight)
        {
            snapOffsetX = snapWorldX - transform.position.x;
            hasTopExit = true;
            topExitX = topExitWorldX;
            topSurfaceY = topSurfaceWorldY;
            topPlatformCellHeight = Mathf.Max(0.1f, Mathf.Abs(platformCellHeight));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || !other.CompareTag(playerTag)) return;

            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement?.SetLadderZone(this, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !other.CompareTag(playerTag)) return;

            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement?.SetLadderZone(this, false);
        }
    }
}
