using UnityEngine;
using UnityEngine.UIElements;

public class ScentIndicatorController : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite; // 在 Inspector 中拖入你的 arrow.png
    [SerializeField] private string indicatorName = "ScentIndicator"; // 确保和 UI Builder 中的名字一致
    private UIDocument uiDocument;
    private VisualElement scentIndicator;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found on this GameObject.");
            return;
        }

        scentIndicator = uiDocument.rootVisualElement.Q<VisualElement>(indicatorName);
        if (scentIndicator == null)
        {
            Debug.LogError($"Could not find VisualElement named '{indicatorName}'.");
            return;
        }

        // 核心：通过脚本动态设置背景图片
        if (arrowSprite != null)
        {
            // 将 Sprite 转换为 Texture2D
            Texture2D texture = arrowSprite.texture;

            // 创建 Background 对象并设置图片
            Background background = new Background();
            background.sprite = arrowSprite; // UI Toolkit 可以直接用 Sprite
                                             // 或者使用 texture（如果上述不行，尝试下面这行）:
                                             // background.texture = texture;

            scentIndicator.style.backgroundImage = new StyleBackground(background);

            Debug.Log("Arrow background image set via script.");
        }
        else
        {
            Debug.LogError("Arrow Sprite is not assigned in the Inspector.");
        }
    }
}