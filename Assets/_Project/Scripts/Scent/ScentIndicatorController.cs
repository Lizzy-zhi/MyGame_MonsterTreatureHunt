using UnityEngine;
using UnityEngine.UIElements;
using MonsterTreasureHunt.Gameplay;

namespace MonsterTreasureHunt.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class ScentIndicatorController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private string indicatorName = "ScentIndicator";

        [Header("Targets")]
        [SerializeField] private Transform player;
        [SerializeField] private TreasureCollectible targetTreasure;

        [Header("Scent Settings")]
        [SerializeField] private float scentRange = 18f;
        [SerializeField] private float edgeMargin = 72f;
        [SerializeField] private float minArrowSize = 38f;
        [SerializeField] private float maxArrowSize = 98f;
        [SerializeField] private float minOpacity = 0.35f;
        [SerializeField] private float maxOpacity = 1f;

        private UIDocument uiDocument;
        private VisualElement indicator;
        private Camera mainCam;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            mainCam = Camera.main;

            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) player = playerObj.transform;
            }

            if (targetTreasure == null)
            {
                targetTreasure = FindObjectOfType<TreasureCollectible>();
            }
        }

        private void Start()
        {
            indicator = uiDocument.rootVisualElement.Q<VisualElement>(indicatorName);
            if (indicator == null)
            {
                Debug.LogError($"Missing VisualElement '{indicatorName}' in HUD UXML.");
                enabled = false;
                return;
            }

            indicator.style.display = DisplayStyle.None;
            indicator.style.position = Position.Absolute;

            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) player = playerObj.transform;
            }

            if (targetTreasure == null)
            {
                targetTreasure = FindObjectOfType<TreasureCollectible>();
            }

            Debug.Log($"[ScentIndicator] Ready. player={(player != null)} treasure={(targetTreasure != null)}");
        }

        private void Update()
        {
            if (player == null || targetTreasure == null || targetTreasure.IsCollected || mainCam == null)
            {
                if (indicator != null) indicator.style.display = DisplayStyle.None;
                return;
            }

            Vector3 treasurePos = targetTreasure.transform.position;
            float distance = Vector2.Distance(player.position, treasurePos);
            if (distance > scentRange)
            {
                indicator.style.display = DisplayStyle.None;
                return;
            }

            VisualElement root = uiDocument.rootVisualElement;
            float panelWidth = root.resolvedStyle.width;
            float panelHeight = root.resolvedStyle.height;
            if (panelWidth <= 1f || panelHeight <= 1f)
            {
                indicator.style.display = DisplayStyle.None;
                return;
            }

            indicator.style.display = DisplayStyle.Flex;

            Vector2 dirWorld = ((Vector2)(treasurePos - player.position)).normalized;
            if (dirWorld.sqrMagnitude < 0.001f)
            {
                dirWorld = Vector2.up;
            }

            Vector2 center = new(panelWidth * 0.5f, panelHeight * 0.5f);

            float tx = Mathf.Abs(dirWorld.x) > 0.001f
                ? (((dirWorld.x > 0f ? panelWidth - edgeMargin : edgeMargin) - center.x) / dirWorld.x)
                : float.MaxValue;
            float ty = Mathf.Abs(dirWorld.y) > 0.001f
                ? (((dirWorld.y > 0f ? panelHeight - edgeMargin : edgeMargin) - center.y) / dirWorld.y)
                : float.MaxValue;

            float t = Mathf.Min(Mathf.Abs(tx), Mathf.Abs(ty));
            Vector2 edgePos = center + dirWorld * t;

            float closeness = Mathf.Clamp01(1f - (distance / scentRange));
            float size = Mathf.Lerp(minArrowSize, maxArrowSize, closeness);
            float opacity = Mathf.Lerp(minOpacity, maxOpacity, closeness);
            float angle = Mathf.Atan2(dirWorld.y, dirWorld.x) * Mathf.Rad2Deg - 90f;

            indicator.style.left = edgePos.x - size * 0.5f;
            indicator.style.top = (panelHeight - edgePos.y) - size * 0.5f;
            indicator.style.width = size;
            indicator.style.height = size;
            indicator.style.rotate = new Rotate(angle);
            indicator.style.opacity = opacity;

            Color tint = Color.Lerp(new Color(0.85f, 0.85f, 0.85f), new Color(0.2f, 0.2f, 0.2f), closeness);
            indicator.style.unityBackgroundImageTintColor = tint;
        }
    }
}
