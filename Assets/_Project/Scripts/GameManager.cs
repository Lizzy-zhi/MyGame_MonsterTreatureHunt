using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentScore = 0;
    private Label scoreLabel;

    void Awake()
    {
        // 单例模式：确保全局唯一
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // 从 UI 获取显示分数的 Label
        var uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument != null)
        {
            scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
            UpdateScoreUI();
        }
    }

    public void AddScore(int value)
    {
        currentScore += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = $"Treasures: {currentScore}";
        }
    }
}
