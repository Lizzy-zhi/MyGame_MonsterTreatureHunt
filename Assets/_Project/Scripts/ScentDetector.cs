using UnityEngine;
using UnityEngine.UIElements;

public class ScentDetector : MonoBehaviour
{
    [SerializeField] private Transform treasureTarget; // 宝藏目标
    [SerializeField] private float maxScentDistance = 20f; // 最大感应距离

    private VisualElement scentIndicator;
    private UIDocument uiDocument;

    void Start()
    {
        Debug.Log("ScentDetector: Start() 开始执行。");
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null)
        {
            Debug.Log("ScentDetector: 找到了 UIDocument 组件。");
            scentIndicator = uiDocument.rootVisualElement.Q<VisualElement>("ScentIndicator");
            if (scentIndicator != null)
            {
                Debug.Log("ScentDetector: 成功找到名为 'ScentIndicator' 的UI元素。");
            }
            else
            {
                Debug.LogError("ScentDetector: 错误！未找到名为 'ScentIndicator' 的UI元素。请检查UI Builder中的元素名称。");
            }
        }
        else
        {
            Debug.LogError("ScentDetector: 错误！当前物体上未找到 UIDocument 组件。");
        }
        // 初始隐藏指示器
        if (scentIndicator != null) scentIndicator.style.display = DisplayStyle.None;
    }

    void Update()
    {
        // 基础空值检查
        if (treasureTarget == null)
        {
            Debug.LogWarning("ScentDetector: treasureTarget 为空，跳过更新。");
            return;
        }
        if (scentIndicator == null)
        {
            Debug.LogWarning("ScentDetector: scentIndicator 为空，跳过更新。");
            return;
        }

        Vector3 direction = treasureTarget.position - transform.position;
        float distance = direction.magnitude;
        Debug.Log($"ScentDetector: 玩家与宝藏的距离 distance = {distance}, 最大感应距离 maxScentDistance = {maxScentDistance}");

        if (distance < maxScentDistance)
        {
            Debug.Log("ScentDetector: 距离在范围内，开始计算箭头状态。");
            scentIndicator.style.display = DisplayStyle.Flex;

            // --- 修改：从绝对坐标改为相对计算 ---
            // 1. 计算归一化的屏幕方向向量
            Vector3 screenPos = Camera.main.WorldToViewportPoint(treasureTarget.position);
            Debug.Log($"ScentDetector: WorldToViewportPoint 结果 screenPos = ({screenPos.x:F3}, {screenPos.y:F3}, {screenPos.z:F3})");

            // 检查目标是否在摄像机后方 (z < 0)
            if (screenPos.z < 0)
            {
                Debug.LogWarning("ScentDetector: 宝藏位于摄像机后方！Viewport 坐标可能无效。");
            }

            Vector2 viewportCenter = new Vector2(0.5f, 0.5f);
            Vector2 dirToScreen = new Vector2(screenPos.x, screenPos.y) - viewportCenter;
            Debug.Log($"ScentDetector: 归一化前方向向量 dirToScreen (raw) = ({dirToScreen.x:F3}, {dirToScreen.y:F3})");

            // 2. 计算旋转角度
            float angle = Mathf.Atan2(dirToScreen.y, dirToScreen.x) * Mathf.Rad2Deg;
            Debug.Log($"ScentDetector: 计算出的旋转角度 angle = {angle:F2} 度");

            // 3. 限制方向向量长度，确保箭头在屏幕内
            float distanceFromCenter = 0.4f;
            dirToScreen = Vector2.ClampMagnitude(dirToScreen, distanceFromCenter);
            Debug.Log($"ScentDetector: 限制长度后方向向量 dirToScreen (clamped) = ({dirToScreen.x:F3}, {dirToScreen.y:F3})");

            // 4. 计算并应用相对位置
            float screenEdgePadding = 0.1f;
            Vector2 viewportPos = viewportCenter + dirToScreen * (1 - screenEdgePadding * 2);
            Debug.Log($"ScentDetector: 最终视口坐标 viewportPos = ({viewportPos.x:F3}, {viewportPos.y:F3})");

            // 应用变换
            scentIndicator.style.rotate = new Rotate(angle);
            scentIndicator.style.translate = new Translate(
                Length.Percent((viewportPos.x - 0.5f) * 200),
                Length.Percent((viewportPos.y - 0.5f) * 200)
            );
            Debug.Log($"ScentDetector: 已应用旋转({angle:F2})和位移。");

            // 保持原有的缩放和透明度逻辑
            float scale = Mathf.Lerp(1.5f, 0.5f, distance / maxScentDistance);
            float opacity = Mathf.Lerp(1f, 0.3f, distance / maxScentDistance);
            scentIndicator.style.scale = new Scale(new Vector3(scale, scale, 1));
            scentIndicator.style.opacity = opacity;
        }
        else
        {
            Debug.Log("ScentDetector: 距离超出范围，隐藏箭头。");
            scentIndicator.style.display = DisplayStyle.None;
        }
    }
}