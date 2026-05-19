using UnityEngine;
using UnityEngine.UIElements;

public class ScentDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;  // 拖拽玩家物体到这里
    [SerializeField] private Transform treasureTarget;   // 拖拽 treasure 到这里
    [SerializeField] private Sprite arrowSprite;         // 拖拽箭头图片到这里

    [Header("Settings")]
    [SerializeField] private float maxScentDistance = 20f;
    [SerializeField] private float arrowRotationOffset = 0f;
    [SerializeField] private bool showDebugLogs = true;

    private VisualElement scentIndicator;
    private UIDocument uiDocument;
    private Camera mainCamera;
    private float lastAngle = 0f;
    private float lastDistance = 0f;

    void Start()
    {
        mainCamera = Camera.main;

        // 查找 UIDocument（可能在别的物体上）
        uiDocument = FindObjectOfType<UIDocument>();

        if (showDebugLogs)
        {
            Debug.Log("ScentDetector: Start() called");
            Debug.Log($"ScentDetector: Player Transform = {(playerTransform != null ? playerTransform.name : "NULL")}");
            Debug.Log($"ScentDetector: Treasure Target = {(treasureTarget != null ? treasureTarget.name : "NULL")}");
        }

        if (uiDocument == null)
        {
            Debug.LogError("ScentDetector: Could not find UIDocument in scene!");
            return;
        }

        scentIndicator = uiDocument.rootVisualElement.Q<VisualElement>("ScentIndicator");
        if (scentIndicator == null)
        {
            Debug.LogError("ScentDetector: Could not find 'ScentIndicator' element! Check UXML.");
            return;
        }
        else if (showDebugLogs)
        {
            Debug.Log("ScentDetector: Found 'ScentIndicator' element");
        }

        if (arrowSprite != null)
        {
            scentIndicator.style.backgroundImage = new StyleBackground(arrowSprite);
        }
        else
        {
            Debug.LogWarning("ScentDetector: Arrow sprite not assigned!");
        }

        // 如果 playerTransform 没设置，就用当前物体
        if (playerTransform == null)
        {
            playerTransform = transform;
            Debug.LogWarning("ScentDetector: Player Transform not set, using this object's transform");
        }

        HideArrow();
    }

    void Update()
    {
        if (scentIndicator == null || mainCamera == null) return;

        if (playerTransform == null || treasureTarget == null)
        {
            if (showDebugLogs && lastDistance > 0)
                Debug.Log("ScentDetector: Player or Treasure is null");
            HideArrow();
            return;
        }

        Vector3 directionToTreasure = treasureTarget.position - playerTransform.position;
        float distance = directionToTreasure.magnitude;

        if (distance >= maxScentDistance)
        {
            if (showDebugLogs && lastDistance < maxScentDistance)
                Debug.Log($"ScentDetector: Distance {distance:F1} > max {maxScentDistance}, hiding");
            HideArrow();
            return;
        }

        ShowArrow();
        UpdateArrowDirection(directionToTreasure);
        UpdateArrowSize(distance);
        UpdateArrowOpacity(distance);
    }

    private void UpdateArrowDirection(Vector3 directionToTreasure)
    {
        // 修复：将 Y 轴取反以解决对称问题
        float angle = Mathf.Atan2(-directionToTreasure.y, directionToTreasure.x) * Mathf.Rad2Deg;
        float finalAngle = angle + arrowRotationOffset;

        if (showDebugLogs && Mathf.Abs(finalAngle - lastAngle) > 0.1f)
        {
            Debug.Log($"ScentDetector: Angle = {finalAngle:F1}° (raw angle={angle:F1}°)");
        }

        scentIndicator.style.rotate = new Rotate(finalAngle);
        lastAngle = finalAngle;
    }

    private void UpdateArrowSize(float distance)
    {
        float normalizedDistance = distance / maxScentDistance;
        float scale = Mathf.Lerp(2.0f, 0.8f, normalizedDistance);
        scentIndicator.style.scale = new Scale(new Vector3(scale, scale, 1));
        lastDistance = distance;
    }

    private void UpdateArrowOpacity(float distance)
    {
        float normalizedDistance = distance / maxScentDistance;
        float opacity = Mathf.Lerp(1.0f, 0.2f, normalizedDistance);
        scentIndicator.style.opacity = opacity;
    }

    private void ShowArrow()
    {
        if (scentIndicator.style.display != DisplayStyle.Flex)
        {
            scentIndicator.style.display = DisplayStyle.Flex;
            if (showDebugLogs) Debug.Log("ScentDetector: Showing arrow");
        }
    }

    private void HideArrow()
    {
        if (scentIndicator != null && scentIndicator.style.display != DisplayStyle.None)
        {
            scentIndicator.style.display = DisplayStyle.None;
            if (showDebugLogs) Debug.Log("ScentDetector: Hiding arrow");
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
