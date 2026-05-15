using UnityEngine;
using UnityEngine.UIElements;

public class ScentDetector : MonoBehaviour
{
    [SerializeField] private Transform treasureTarget; // 拖拽宝藏到这里
    [SerializeField] private float maxScentDistance = 20f; // 最大感应距离

    private VisualElement scentIndicator;
    private UIDocument uiDocument;

    void Start()
    {
        uiDocument = GetComponentInParent<UIDocument>();
        if (uiDocument != null)
        {
            scentIndicator = uiDocument.rootVisualElement.Q<VisualElement>("ScentIndicator");
        }
        // 初始隐藏指示器
        if (scentIndicator != null) scentIndicator.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (treasureTarget == null || scentIndicator == null) return;

        Vector3 direction = treasureTarget.position - transform.position;
        float distance = direction.magnitude;

        // 在感应范围内
        if (distance < maxScentDistance)
        {
            scentIndicator.style.display = DisplayStyle.Flex; // 显示

            // 1. 计算屏幕空间方向
            Vector2 screenPos = Camera.main.WorldToScreenPoint(treasureTarget.position);
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 dirToScreen = (screenPos - screenCenter).normalized;

            // 2. 更新箭头位置（固定在屏幕边缘）
            float edgeX = (Screen.width / 2 - 30) * dirToScreen.x;
            float edgeY = (Screen.height / 2 - 30) * dirToScreen.y;
            scentIndicator.style.translate = new Translate(edgeX, edgeY);

            // 3. 更新箭头旋转（指向宝藏）
            float angle = Mathf.Atan2(dirToScreen.y, dirToScreen.x) * Mathf.Rad2Deg;
            scentIndicator.style.rotate = new Rotate(angle - 90f); // -90 修正箭头朝向

            // 4. 更新箭头大小/透明度（距离越近越明显）
            float scale = Mathf.Lerp(1.5f, 0.5f, distance / maxScentDistance);
            float opacity = Mathf.Lerp(1f, 0.3f, distance / maxScentDistance);
            scentIndicator.style.scale = new Scale(new Vector3(scale, scale, 1));
            scentIndicator.style.opacity = opacity;
        }
        else
        {
            scentIndicator.style.display = DisplayStyle.None; // 超出范围隐藏
        }
    }
}
