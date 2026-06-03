using UnityEngine;
using UnityEngine.UIElements;
using MonsterTreasureHunt.Levels;
using MonsterTreasureHunt.Player;

namespace MonsterTreasureHunt.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HUDManager : MonoBehaviour
    {
        [Header("UI Element Names")]
        [SerializeField] private string startPanelName = "StartPanel";
        [SerializeField] private string startButtonName = "StartButton";
        [SerializeField] private string startQuitButtonName = "StartQuitButton";
        [SerializeField] private string mapSelectPanelName = "MapSelectPanel";
        [SerializeField] private string beginnerMapButtonName = "BeginnerMapButton";
        [SerializeField] private string foggyMapButtonName = "FoggyMapButton";
        [SerializeField] private string volcanoMapButtonName = "VolcanoMapButton";
        [SerializeField] private string colorSelectPanelName = "ColorSelectPanel";
        [SerializeField] private string purpleColorButtonName = "ColorPurpleButton";
        [SerializeField] private string greenColorButtonName = "ColorGreenButton";
        [SerializeField] private string pinkColorButtonName = "ColorPinkButton";
        [SerializeField] private string settingsButtonName = "SettingsButton";
        [SerializeField] private string settingsPanelName = "SettingsPanel";
        [SerializeField] private string helpButtonName = "HelpButton";
        [SerializeField] private string continueButtonName = "ContinueButton";
        [SerializeField] private string escapeButtonName = "EscapeButton";
        [SerializeField] private string rulesPanelName = "RulesPanel";
        [SerializeField] private string rulesLabelName = "RulesLabel";
        [SerializeField] private string resultLabelName = "ResultLabel";

        [Header("Level")]
        [SerializeField] private BeginnerIslandLevelController levelController;
        [SerializeField] private BeginnerIslandMapBuilder mapBuilder;
        [SerializeField] private PlayerMovement playerMovement;

        private VisualElement startPanel;
        private Button startButton;
        private Button startQuitButton;
        private VisualElement mapSelectPanel;
        private Button beginnerMapButton;
        private Button foggyMapButton;
        private Button volcanoMapButton;
        private VisualElement colorSelectPanel;
        private Button purpleColorButton;
        private Button greenColorButton;
        private Button pinkColorButton;
        private Button settingsButton;
        private Button helpButton;
        private Button continueButton;
        private Button escapeButton;
        private VisualElement settingsPanel;
        private VisualElement rulesPanel;
        private Label rulesLabel;
        private Label resultLabel;

        private bool levelCompleted;
        private bool gameStarted;
        private Rigidbody2D playerBody;
        private BeginnerIslandMapBuilder.MapTheme pendingMap = BeginnerIslandMapBuilder.MapTheme.BeginnerIsland;
        private bool mapChosen;
        private string selectedMapTitle = "Beginner Island";

        private const string RulesText =
            "Move left and right with A / D or the arrow keys.\n" +
            "Press Space to jump over small steps.\n" +
            "Follow the scent arrow when the treasure is off screen.\n" +
            "Reach the treasure chest at the far right side of the island.";

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

            UnregisterCallbacks();
        }

        private void Start()
        {
            UIDocument doc = GetComponent<UIDocument>();
            VisualElement root = doc.rootVisualElement;

            startPanel = root.Q<VisualElement>(startPanelName);
            startButton = root.Q<Button>(startButtonName);
            startQuitButton = root.Q<Button>(startQuitButtonName);
            mapSelectPanel = root.Q<VisualElement>(mapSelectPanelName);
            beginnerMapButton = root.Q<Button>(beginnerMapButtonName);
            foggyMapButton = root.Q<Button>(foggyMapButtonName);
            volcanoMapButton = root.Q<Button>(volcanoMapButtonName);
            colorSelectPanel = root.Q<VisualElement>(colorSelectPanelName);
            purpleColorButton = root.Q<Button>(purpleColorButtonName);
            greenColorButton = root.Q<Button>(greenColorButtonName);
            pinkColorButton = root.Q<Button>(pinkColorButtonName);
            settingsButton = root.Q<Button>(settingsButtonName);
            settingsPanel = root.Q<VisualElement>(settingsPanelName);
            helpButton = root.Q<Button>(helpButtonName);
            continueButton = root.Q<Button>(continueButtonName);
            escapeButton = root.Q<Button>(escapeButtonName);
            rulesPanel = root.Q<VisualElement>(rulesPanelName);
            rulesLabel = root.Q<Label>(rulesLabelName);
            resultLabel = root.Q<Label>(resultLabelName);

            // Prevent gameplay keys (especially Space) from triggering focused UI buttons.
            DisableKeyboardFocus(settingsButton);
            DisableKeyboardFocus(helpButton);
            DisableKeyboardFocus(continueButton);
            DisableKeyboardFocus(escapeButton);
            DisableKeyboardFocus(startButton);
            DisableKeyboardFocus(startQuitButton);
            DisableKeyboardFocus(beginnerMapButton);
            DisableKeyboardFocus(foggyMapButton);
            DisableKeyboardFocus(volcanoMapButton);
            DisableKeyboardFocus(purpleColorButton);
            DisableKeyboardFocus(greenColorButton);
            DisableKeyboardFocus(pinkColorButton);
            root.Focus();

            if (mapBuilder == null)
            {
                mapBuilder = FindObjectOfType<BeginnerIslandMapBuilder>();
            }

            if (playerMovement == null)
            {
                playerMovement = FindObjectOfType<PlayerMovement>();
            }

            if (playerMovement != null)
            {
                playerBody = playerMovement.GetComponent<Rigidbody2D>();
            }

            RegisterCallbacks();

            if (rulesLabel != null)
            {
                rulesLabel.text = RulesText;
            }

            gameStarted = false;
            mapChosen = false;
            SetStartPanelVisible(true);
            SetMapSelectVisible(false);
            SetColorSelectVisible(false);
            SetSettingsButtonVisible(false);
            SetGameplayInputEnabled(false);
            SetSettingsVisible(false);
            SetRulesVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }
        }

        private static void DisableKeyboardFocus(VisualElement element)
        {
            if (element == null) return;
            element.focusable = false;
            element.tabIndex = -1;
        }

        private void RegisterCallbacks()
        {
            if (startButton != null) startButton.clicked += StartGame;
            if (startQuitButton != null) startQuitButton.clicked += EscapeGame;
            if (beginnerMapButton != null) beginnerMapButton.clicked += SelectBeginnerMap;
            if (foggyMapButton != null) foggyMapButton.clicked += SelectFoggyMap;
            if (volcanoMapButton != null) volcanoMapButton.clicked += SelectVolcanoMap;
            if (purpleColorButton != null) purpleColorButton.clicked += SelectPurpleColor;
            if (greenColorButton != null) greenColorButton.clicked += SelectGreenColor;
            if (pinkColorButton != null) pinkColorButton.clicked += SelectPinkColor;
            if (settingsButton != null) settingsButton.clicked += ToggleSettings;
            if (helpButton != null) helpButton.clicked += ShowRules;
            if (continueButton != null) continueButton.clicked += ContinueGame;
            if (escapeButton != null) escapeButton.clicked += EscapeGame;
        }

        private void UnregisterCallbacks()
        {
            if (startButton != null) startButton.clicked -= StartGame;
            if (startQuitButton != null) startQuitButton.clicked -= EscapeGame;
            if (beginnerMapButton != null) beginnerMapButton.clicked -= SelectBeginnerMap;
            if (foggyMapButton != null) foggyMapButton.clicked -= SelectFoggyMap;
            if (volcanoMapButton != null) volcanoMapButton.clicked -= SelectVolcanoMap;
            if (purpleColorButton != null) purpleColorButton.clicked -= SelectPurpleColor;
            if (greenColorButton != null) greenColorButton.clicked -= SelectGreenColor;
            if (pinkColorButton != null) pinkColorButton.clicked -= SelectPinkColor;
            if (settingsButton != null) settingsButton.clicked -= ToggleSettings;
            if (helpButton != null) helpButton.clicked -= ShowRules;
            if (continueButton != null) continueButton.clicked -= ContinueGame;
            if (escapeButton != null) escapeButton.clicked -= EscapeGame;
        }

        private void StartGame()
        {
            mapChosen = false;
            SetStartPanelVisible(false);
            SetMapSelectVisible(true);
            SetColorSelectVisible(false);
            SetSettingsButtonVisible(false);
            SetSettingsVisible(false);
            SetRulesVisible(false);
        }

        private void SelectBeginnerMap()
        {
            SelectMap(BeginnerIslandMapBuilder.MapTheme.BeginnerIsland, "Beginner Island");
        }

        private void SelectFoggyMap()
        {
            SelectMap(BeginnerIslandMapBuilder.MapTheme.FoggyForest, "Foggy Forest");
        }

        private void SelectVolcanoMap()
        {
            SelectMap(BeginnerIslandMapBuilder.MapTheme.VolcanoCave, "Volcano Cave");
        }

        private void SelectMap(BeginnerIslandMapBuilder.MapTheme map, string mapTitle)
        {
            pendingMap = map;
            selectedMapTitle = mapTitle;
            mapChosen = true;
            SetMapSelectVisible(false);
            SetColorSelectVisible(true);
        }

        private void SelectPurpleColor()
        {
            StartWithColor(new Color(0.74f, 0.58f, 1f, 1f));
        }

        private void SelectGreenColor()
        {
            StartWithColor(new Color(0.6f, 0.95f, 0.58f, 1f));
        }

        private void SelectPinkColor()
        {
            StartWithColor(new Color(1f, 0.68f, 0.84f, 1f));
        }

        private void StartWithColor(Color tint)
        {
            if (!mapChosen) return;

            if (mapBuilder != null)
            {
                mapBuilder.SelectMap(pendingMap);
                mapBuilder.BuildMap();
            }

            if (playerMovement != null)
            {
                playerMovement.SetBodyTint(tint);
            }

            gameStarted = true;
            SetColorSelectVisible(false);
            SetSettingsButtonVisible(true);
            SetGameplayInputEnabled(true);
            SetSettingsVisible(false);
            SetRulesVisible(false);
        }

        private void ToggleSettings()
        {
            if (!gameStarted || levelCompleted || settingsPanel == null) return;

            bool isVisible = settingsPanel.style.display == DisplayStyle.Flex;
            SetSettingsVisible(!isVisible);
            if (isVisible)
            {
                SetRulesVisible(false);
            }
        }

        private void ShowRules()
        {
            if (!gameStarted || levelCompleted) return;

            SetSettingsVisible(true);
            SetRulesVisible(true);
        }

        private void ContinueGame()
        {
            if (!gameStarted) return;
            SetSettingsVisible(false);
            SetRulesVisible(false);
        }

        private void EscapeGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void HandleLevelCompleted()
        {
            levelCompleted = true;
            SetSettingsVisible(false);
            SetRulesVisible(false);

            if (resultLabel != null)
            {
                resultLabel.text = selectedMapTitle + " cleared!\nYou found your treasure.";
                resultLabel.style.display = DisplayStyle.Flex;
            }
        }

        private void SetStartPanelVisible(bool visible)
        {
            if (startPanel != null)
            {
                startPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetMapSelectVisible(bool visible)
        {
            if (mapSelectPanel != null)
            {
                mapSelectPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetColorSelectVisible(bool visible)
        {
            if (colorSelectPanel != null)
            {
                colorSelectPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetSettingsButtonVisible(bool visible)
        {
            if (settingsButton != null)
            {
                settingsButton.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetGameplayInputEnabled(bool enabled)
        {
            if (playerMovement != null)
            {
                playerMovement.enabled = enabled;
            }

            if (playerBody != null)
            {
                playerBody.velocity = Vector2.zero;
                playerBody.angularVelocity = 0f;
                playerBody.simulated = enabled;
            }
        }

        private void SetSettingsVisible(bool visible)
        {
            if (settingsPanel != null)
            {
                settingsPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetRulesVisible(bool visible)
        {
            if (rulesPanel != null)
            {
                rulesPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
