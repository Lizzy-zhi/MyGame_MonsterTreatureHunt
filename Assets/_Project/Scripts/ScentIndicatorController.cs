using UnityEngine;
using UnityEngine.UIElements;

public class ScentIndicatorController : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private string indicatorName = "ScentIndicator";
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

        if (arrowSprite != null)
        {
            scentIndicator.style.backgroundImage = new StyleBackground(arrowSprite);
            Debug.Log("Arrow background image set via script.");
        }
        else
        {
            Debug.LogError("Arrow Sprite is not assigned in the Inspector.");
        }
    }
}

