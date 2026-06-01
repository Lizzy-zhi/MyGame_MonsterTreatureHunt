using UnityEngine;
using UnityEngine.UIElements;
using MonsterTreasureHunt.Levels;

namespace MonsterTreasureHunt.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HUDManager : MonoBehaviour
    {
        [Header("UI Element Names")]
        [SerializeField] private string helpButtonName = "HelpButton";
        [SerializeField] private string tutorialLabelName = "TutorialLabel";
        [SerializeField] private string resultLabelName = "ResultLabel";

        [Header("Level")]
        [SerializeField] private BeginnerIslandLevelController levelController;

        private Button helpButton;
        private Label tutorialLabel;
        private Label resultLabel;

        private bool moved;
        private bool jumped;
        private bool helpExpanded;
        private bool levelCompleted;

        private void OnEnable()
        {
            if (levelController != null)
            {
                levelController.LevelCompleted += HandleLevelCompleted;
            }
        }

        private void OnDisable()
        {
            if (levelController != null)
            {
                levelController.LevelCompleted -= HandleLevelCompleted;
            }

            if (helpButton != null)
            {
                helpButton.clicked -= ToggleHelp;
            }
        }

        private void Start()
        {
            UIDocument doc = GetComponent<UIDocument>();
            VisualElement root = doc.rootVisualElement;
            helpButton = root.Q<Button>(helpButtonName);
            tutorialLabel = root.Q<Label>(tutorialLabelName);
            resultLabel = root.Q<Label>(resultLabelName);

            helpExpanded = false;

            if (helpButton != null)
            {
                helpButton.text = "Help";
                helpButton.clicked += ToggleHelp;
            }

            if (tutorialLabel != null)
            {
                tutorialLabel.text = string.Empty;
                tutorialLabel.style.display = DisplayStyle.None;
            }

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }
        }

        private void Update()
        {
            if (levelCompleted || !helpExpanded) return;

            if (!moved && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
            {
                moved = true;
                RefreshTutorialText();
            }

            if (moved && !jumped && Input.GetButtonDown("Jump"))
            {
                jumped = true;
                RefreshTutorialText();
            }
        }

        private void HandleLevelCompleted()
        {
            levelCompleted = true;
            helpExpanded = false;

            if (tutorialLabel != null)
            {
                tutorialLabel.style.display = DisplayStyle.None;
            }

            if (resultLabel != null)
            {
                resultLabel.text = "Beginner Island cleared!\nYou found your first treasure.";
                resultLabel.style.display = DisplayStyle.Flex;
            }

            if (helpButton != null)
            {
                helpButton.text = "Help";
            }
        }

        private void ToggleHelp()
        {
            helpExpanded = !helpExpanded;
            RefreshTutorialText();
        }

        private void RefreshTutorialText()
        {
            if (levelCompleted) return;

            if (helpButton != null)
            {
                helpButton.text = helpExpanded ? "Hide" : "Help";
            }

            if (tutorialLabel == null) return;

            if (!helpExpanded)
            {
                tutorialLabel.style.display = DisplayStyle.None;
                return;
            }

            tutorialLabel.style.display = DisplayStyle.Flex;

            if (!moved)
            {
                tutorialLabel.text = "Move with A/D or ←/→";
            }
            else if (!jumped)
            {
                tutorialLabel.text = "Nice! Press Space to jump.";
            }
            else
            {
                tutorialLabel.text = "Follow the scent arrow to find treasure.";
            }
        }
    }
}
