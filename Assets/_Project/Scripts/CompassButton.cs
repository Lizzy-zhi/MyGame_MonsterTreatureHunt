using UnityEngine;
using UnityEngine.UI;

public class CompassButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform treasureTarget;
    [SerializeField] private Image arrowImage;
    [SerializeField] private GameObject compassPanel;

    [Header("Settings")]
    [SerializeField] private float maxScentDistance = 20f;
    [SerializeField] private float arrowRotationOffset = 0f;
    [SerializeField] private bool showDebugLogs = true;

    private float lastAngle = 0f;
    private float lastDistance = 0f;
    private bool isPanelOpen = false;

    void Awake()
    {
        if (showDebugLogs)
            Debug.Log("CompassButton: Awake() called");
    }

    void Start()
    {
        if (showDebugLogs)
            Debug.Log("CompassButton: Start() called");

        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerMovement>()?.transform;
            if (playerTransform == null)
            {
                Debug.LogWarning("CompassButton: Player Transform not set, trying to find Player");
            }
        }

        if (treasureTarget == null)
        {
            Treasure[] treasures = FindObjectsOfType<Treasure>();
            if (treasures.Length > 0)
            {
                treasureTarget = treasures[0].transform;
                if (showDebugLogs)
                    Debug.Log($"CompassButton: Using first treasure as target: {treasureTarget.name}");
            }
        }

        if (compassPanel != null)
        {
            compassPanel.SetActive(false);
            if (showDebugLogs)
                Debug.Log("CompassButton: CompassPanel hidden on start");
        }
        else
        {
            Debug.LogError("CompassButton: compassPanel reference is NULL!");
        }

        // 移除 Button 组件的事件绑定，我们使用 Update 中的直接点击检测
    }

    void Update()
    {
        // 检测鼠标点击按钮（直接检测，绕过 Button 组件的事件系统）
        if (Input.GetMouseButtonDown(0))
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 mousePos = Input.mousePosition;
            Vector2 buttonPos = rectTransform.position;

            float halfWidth = rectTransform.rect.width / 2;
            float halfHeight = rectTransform.rect.height / 2;
            bool isInside = mousePos.x >= buttonPos.x - halfWidth &&
                           mousePos.x <= buttonPos.x + halfWidth &&
                           mousePos.y >= buttonPos.y - halfHeight &&
                           mousePos.y <= buttonPos.y + halfHeight;

            if (isInside)
            {
                TogglePanel();
            }
        }

        if (!isPanelOpen || arrowImage == null) return;

        if (playerTransform == null)
        {
            HideArrow();
            return;
        }

        // 动态查找最近的可用 treasure
        UpdateNearestTreasure();

        if (treasureTarget == null)
        {
            HideArrow();
            return;
        }

        Vector3 directionToTreasure = treasureTarget.position - playerTransform.position;
        float distance = directionToTreasure.magnitude;

        if (distance >= maxScentDistance)
        {
            HideArrow();
            return;
        }

        ShowArrow();
        UpdateArrowDirection(directionToTreasure);
        UpdateArrowSize(distance);
        UpdateArrowOpacity(distance);
    }

    private void UpdateNearestTreasure()
    {
        Treasure[] treasures = FindObjectsOfType<Treasure>();

        if (treasures.Length == 0)
        {
            treasureTarget = null;
            return;
        }

        Transform nearestTreasure = null;
        float nearestDistance = float.MaxValue;

        foreach (Treasure treasure in treasures)
        {
            if (treasure == null) continue;

            float distance = Vector3.Distance(playerTransform.position, treasure.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTreasure = treasure.transform;
            }
        }

        treasureTarget = nearestTreasure;
    }

    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;

        if (compassPanel != null)
        {
            compassPanel.SetActive(isPanelOpen);
        }
    }

    private void UpdateArrowDirection(Vector3 directionToTreasure)
    {
        // 修复：移除负号，正确计算朝向角度
        float angle = Mathf.Atan2(directionToTreasure.y, directionToTreasure.x) * Mathf.Rad2Deg;
        float finalAngle = angle + arrowRotationOffset;

        arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, finalAngle);
        lastAngle = finalAngle;
    }

    private void UpdateArrowSize(float distance)
    {
        float normalizedDistance = distance / maxScentDistance;
        float scale = Mathf.Lerp(2.0f, 0.8f, normalizedDistance);
        arrowImage.rectTransform.localScale = new Vector3(scale, scale, 1);
        lastDistance = distance;
    }

    private void UpdateArrowOpacity(float distance)
    {
        float normalizedDistance = distance / maxScentDistance;
        float opacity = Mathf.Lerp(1.0f, 0.2f, normalizedDistance);
        Color color = arrowImage.color;
        color.a = opacity;
        arrowImage.color = color;
    }

    private void ShowArrow()
    {
        if (arrowImage.enabled != true)
        {
            arrowImage.enabled = true;
            if (showDebugLogs) Debug.Log("CompassButton: Showing arrow");
        }
    }

    private void HideArrow()
    {
        if (arrowImage.enabled != false)
        {
            arrowImage.enabled = false;
            if (showDebugLogs) Debug.Log("CompassButton: Hiding arrow");
        }
    }

    public void SetTreasureTarget(Transform newTarget)
    {
        treasureTarget = newTarget;
    }

    public void ClearTreasureTarget()
    {
        treasureTarget = null;
        HideArrow();
    }
}