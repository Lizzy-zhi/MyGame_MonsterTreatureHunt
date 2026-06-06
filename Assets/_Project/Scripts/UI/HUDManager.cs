using UnityEngine;
using UnityEngine.UIElements;
using MonsterTreasureHunt.Gameplay;
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
        [SerializeField] private string skinSelectPanelName = "SkinSelectPanel";
        [SerializeField] private string skinPreviewImageName = "SkinPreviewImage";
        [SerializeField] private string skinNameLabelName = "SkinNameLabel";
        [SerializeField] private string purpleSkinButtonName = "PurpleSkinButton";
        [SerializeField] private string greenSkinButtonName = "GreenSkinButton";
        [SerializeField] private string pinkSkinButtonName = "PinkSkinButton";
        [SerializeField] private string yellowSkinButtonName = "YellowSkinButton";
        [SerializeField] private string beigeSkinButtonName = "BeigeSkinButton";
        [SerializeField] private string confirmSkinButtonName = "ConfirmSkinButton";
        [SerializeField] private string backToMapButtonName = "BackToMapButton";
        [SerializeField] private string settingsButtonName = "SettingsButton";
        [SerializeField] private string settingsPanelName = "SettingsPanel";
        [SerializeField] private string helpButtonName = "HelpButton";
        [SerializeField] private string continueButtonName = "ContinueButton";
        [SerializeField] private string escapeButtonName = "EscapeButton";
        [SerializeField] private string rulesPanelName = "RulesPanel";
        [SerializeField] private string rulesLabelName = "RulesLabel";
        [SerializeField] private string resultLabelName = "ResultLabel";
        [SerializeField] private string failureIconName = "FailureIcon";
        [SerializeField] private string livesContainerName = "LivesContainer";
        [SerializeField] private string[] lifeHeartNames = { "LifeHeart1", "LifeHeart2", "LifeHeart3" };

        [Header("Level")]
        [SerializeField] private BeginnerIslandLevelController levelController;
        [SerializeField] private BeginnerIslandMapBuilder mapBuilder;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private float fallFailureDistance = 12f;
        [SerializeField] private int maxLives = 3;
        [SerializeField] private float respawnBackDistance = 4f;
        [SerializeField] private float safePositionSampleInterval = 0.25f;
        [SerializeField] private float respawnInvulnerabilityTime = 0.75f;

        [Header("Lives HUD")]
        [SerializeField] private Sprite fullHeartSprite;
        [SerializeField] private Sprite emptyHeartSprite;

        [Header("Purple Skin")]
        [SerializeField] private Sprite purpleIdleSprite;
        [SerializeField] private Sprite purpleRunSpriteA;
        [SerializeField] private Sprite purpleRunSpriteB;
        [SerializeField] private Sprite purpleJumpSprite;
        [SerializeField] private Sprite purpleCrouchSprite;

        [Header("Green Skin")]
        [SerializeField] private Sprite greenIdleSprite;
        [SerializeField] private Sprite greenRunSpriteA;
        [SerializeField] private Sprite greenRunSpriteB;
        [SerializeField] private Sprite greenJumpSprite;
        [SerializeField] private Sprite greenCrouchSprite;

        [Header("Pink Skin")]
        [SerializeField] private Sprite pinkIdleSprite;
        [SerializeField] private Sprite pinkRunSpriteA;
        [SerializeField] private Sprite pinkRunSpriteB;
        [SerializeField] private Sprite pinkJumpSprite;
        [SerializeField] private Sprite pinkCrouchSprite;

        [Header("Yellow Skin")]
        [SerializeField] private Sprite yellowIdleSprite;
        [SerializeField] private Sprite yellowRunSpriteA;
        [SerializeField] private Sprite yellowRunSpriteB;
        [SerializeField] private Sprite yellowJumpSprite;
        [SerializeField] private Sprite yellowCrouchSprite;

        [Header("Beige Skin")]
        [SerializeField] private Sprite beigeIdleSprite;
        [SerializeField] private Sprite beigeRunSpriteA;
        [SerializeField] private Sprite beigeRunSpriteB;
        [SerializeField] private Sprite beigeJumpSprite;
        [SerializeField] private Sprite beigeCrouchSprite;

        private VisualElement startPanel;
        private Button startButton;
        private Button startQuitButton;
        private VisualElement mapSelectPanel;
        private Button beginnerMapButton;
        private Button foggyMapButton;
        private Button volcanoMapButton;
        private VisualElement skinSelectPanel;
        private Image skinPreviewImage;
        private Label skinNameLabel;
        private Button purpleSkinButton;
        private Button greenSkinButton;
        private Button pinkSkinButton;
        private Button yellowSkinButton;
        private Button beigeSkinButton;
        private Button confirmSkinButton;
        private Button backToMapButton;
        private Button settingsButton;
        private Button helpButton;
        private Button continueButton;
        private Button escapeButton;
        private VisualElement settingsPanel;
        private VisualElement rulesPanel;
        private Label rulesLabel;
        private Label resultLabel;
        private Label failureIcon;
        private VisualElement livesContainer;
        private Image[] lifeHearts;

        private bool levelCompleted;
        private bool levelFailed;
        private bool gameStarted;
        private Rigidbody2D playerBody;
        private float playerStartY;
        private Vector3 lastSafeRespawnPosition;
        private float lastSafeSampleTime;
        private float nextFallDamageTime;
        private bool healthCallbacksRegistered;
        private BeginnerIslandMapBuilder.MapTheme pendingMap = BeginnerIslandMapBuilder.MapTheme.BeginnerIsland;
        private bool mapChosen;
        private string selectedMapTitle = "Beginner Island";
        private SkinChoice selectedSkin;
        private SkinChoice[] skinChoices;

        private const string SelectedSkinClass = "skin-option-selected";
        private const string RulesText =
            "Move left and right with A / D or the arrow keys.\n" +
            "Press Space to jump over small steps.\n" +
            "Follow the scent arrow when the treasure is off screen.\n" +
            "You have three lives. Falling costs one life and returns you to safe ground.\n" +
            "Reach the treasure chest at the far right side of the island.";

        private struct SkinChoice
        {
            public string Name;
            public Sprite Idle;
            public Sprite RunA;
            public Sprite RunB;
            public Sprite Jump;
            public Sprite Crouch;
            public Button Button;

            public SkinChoice(string name, Sprite idle, Sprite runA, Sprite runB, Sprite jump, Sprite crouch, Button button)
            {
                Name = name;
                Idle = idle;
                RunA = runA;
                RunB = runB;
                Jump = jump;
                Crouch = crouch;
                Button = button;
            }
        }

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
            skinSelectPanel = root.Q<VisualElement>(skinSelectPanelName);
            skinPreviewImage = root.Q<Image>(skinPreviewImageName);
            skinNameLabel = root.Q<Label>(skinNameLabelName);
            purpleSkinButton = root.Q<Button>(purpleSkinButtonName);
            greenSkinButton = root.Q<Button>(greenSkinButtonName);
            pinkSkinButton = root.Q<Button>(pinkSkinButtonName);
            yellowSkinButton = root.Q<Button>(yellowSkinButtonName);
            beigeSkinButton = root.Q<Button>(beigeSkinButtonName);
            confirmSkinButton = root.Q<Button>(confirmSkinButtonName);
            backToMapButton = root.Q<Button>(backToMapButtonName);
            settingsButton = root.Q<Button>(settingsButtonName);
            settingsPanel = root.Q<VisualElement>(settingsPanelName);
            helpButton = root.Q<Button>(helpButtonName);
            continueButton = root.Q<Button>(continueButtonName);
            escapeButton = root.Q<Button>(escapeButtonName);
            rulesPanel = root.Q<VisualElement>(rulesPanelName);
            rulesLabel = root.Q<Label>(rulesLabelName);
            resultLabel = root.Q<Label>(resultLabelName);
            failureIcon = root.Q<Label>(failureIconName);
            livesContainer = root.Q<VisualElement>(livesContainerName);
            lifeHearts = BuildLifeHeartElements(root);

            DisableKeyboardFocus(settingsButton);
            DisableKeyboardFocus(helpButton);
            DisableKeyboardFocus(continueButton);
            DisableKeyboardFocus(escapeButton);
            DisableKeyboardFocus(startButton);
            DisableKeyboardFocus(startQuitButton);
            DisableKeyboardFocus(beginnerMapButton);
            DisableKeyboardFocus(foggyMapButton);
            DisableKeyboardFocus(volcanoMapButton);
            DisableKeyboardFocus(purpleSkinButton);
            DisableKeyboardFocus(greenSkinButton);
            DisableKeyboardFocus(pinkSkinButton);
            DisableKeyboardFocus(yellowSkinButton);
            DisableKeyboardFocus(beigeSkinButton);
            DisableKeyboardFocus(confirmSkinButton);
            DisableKeyboardFocus(backToMapButton);
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
                if (playerHealth == null)
                {
                    playerHealth = playerMovement.GetComponent<PlayerHealth>();
                    if (playerHealth == null)
                    {
                        playerHealth = playerMovement.gameObject.AddComponent<PlayerHealth>();
                    }
                }
            }

            if (playerHealth == null)
            {
                playerHealth = FindObjectOfType<PlayerHealth>();
            }

            BuildSkinChoices();
            RegisterCallbacks();

            if (rulesLabel != null)
            {
                rulesLabel.text = RulesText;
            }

            gameStarted = false;
            levelCompleted = false;
            levelFailed = false;
            mapChosen = false;
            SelectSkin(0);
            SetStartPanelVisible(true);
            SetMapSelectVisible(false);
            SetSkinSelectVisible(false);
            SetSettingsButtonVisible(false);
            SetLivesVisible(false);
            SetGameplayInputEnabled(false);
            SetSettingsVisible(false);
            SetRulesVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }

            SetFailureIconVisible(false);
        }

        private void Update()
        {
            UpdateSafeRespawnPosition();
            CheckFallFailure();
        }

        private static void DisableKeyboardFocus(VisualElement element)
        {
            if (element == null) return;
            element.focusable = false;
            element.tabIndex = -1;
        }

        private void BuildSkinChoices()
        {
            skinChoices = new[]
            {
                new SkinChoice("Purple", purpleIdleSprite, purpleRunSpriteA, purpleRunSpriteB, purpleJumpSprite, purpleCrouchSprite, purpleSkinButton),
                new SkinChoice("Green", greenIdleSprite, greenRunSpriteA, greenRunSpriteB, greenJumpSprite, greenCrouchSprite, greenSkinButton),
                new SkinChoice("Pink", pinkIdleSprite, pinkRunSpriteA, pinkRunSpriteB, pinkJumpSprite, pinkCrouchSprite, pinkSkinButton),
                new SkinChoice("Yellow", yellowIdleSprite, yellowRunSpriteA, yellowRunSpriteB, yellowJumpSprite, yellowCrouchSprite, yellowSkinButton),
                new SkinChoice("Beige", beigeIdleSprite, beigeRunSpriteA, beigeRunSpriteB, beigeJumpSprite, beigeCrouchSprite, beigeSkinButton),
            };
        }

        private void RegisterCallbacks()
        {
            if (startButton != null) startButton.clicked += StartGame;
            if (startQuitButton != null) startQuitButton.clicked += EscapeGame;
            if (beginnerMapButton != null) beginnerMapButton.clicked += SelectBeginnerMap;
            if (foggyMapButton != null) foggyMapButton.clicked += SelectFoggyMap;
            if (volcanoMapButton != null) volcanoMapButton.clicked += SelectVolcanoMap;
            if (purpleSkinButton != null) purpleSkinButton.clicked += SelectPurpleSkin;
            if (greenSkinButton != null) greenSkinButton.clicked += SelectGreenSkin;
            if (pinkSkinButton != null) pinkSkinButton.clicked += SelectPinkSkin;
            if (yellowSkinButton != null) yellowSkinButton.clicked += SelectYellowSkin;
            if (beigeSkinButton != null) beigeSkinButton.clicked += SelectBeigeSkin;
            if (confirmSkinButton != null) confirmSkinButton.clicked += ConfirmSkinAndStart;
            if (backToMapButton != null) backToMapButton.clicked += BackToMapSelect;
            if (settingsButton != null) settingsButton.clicked += ToggleSettings;
            if (helpButton != null) helpButton.clicked += ShowRules;
            if (continueButton != null) continueButton.clicked += ContinueGame;
            if (escapeButton != null) escapeButton.clicked += EscapeGame;
            RegisterHealthCallbacks();
        }

        private void UnregisterCallbacks()
        {
            if (startButton != null) startButton.clicked -= StartGame;
            if (startQuitButton != null) startQuitButton.clicked -= EscapeGame;
            if (beginnerMapButton != null) beginnerMapButton.clicked -= SelectBeginnerMap;
            if (foggyMapButton != null) foggyMapButton.clicked -= SelectFoggyMap;
            if (volcanoMapButton != null) volcanoMapButton.clicked -= SelectVolcanoMap;
            if (purpleSkinButton != null) purpleSkinButton.clicked -= SelectPurpleSkin;
            if (greenSkinButton != null) greenSkinButton.clicked -= SelectGreenSkin;
            if (pinkSkinButton != null) pinkSkinButton.clicked -= SelectPinkSkin;
            if (yellowSkinButton != null) yellowSkinButton.clicked -= SelectYellowSkin;
            if (beigeSkinButton != null) beigeSkinButton.clicked -= SelectBeigeSkin;
            if (confirmSkinButton != null) confirmSkinButton.clicked -= ConfirmSkinAndStart;
            if (backToMapButton != null) backToMapButton.clicked -= BackToMapSelect;
            if (settingsButton != null) settingsButton.clicked -= ToggleSettings;
            if (helpButton != null) helpButton.clicked -= ShowRules;
            if (continueButton != null) continueButton.clicked -= ContinueGame;
            if (escapeButton != null) escapeButton.clicked -= EscapeGame;
            UnregisterHealthCallbacks();
        }

        private void RegisterHealthCallbacks()
        {
            if (playerHealth == null || healthCallbacksRegistered) return;

            playerHealth.HealthChanged += HandleHealthChanged;
            healthCallbacksRegistered = true;
        }

        private void UnregisterHealthCallbacks()
        {
            if (playerHealth == null || !healthCallbacksRegistered) return;

            playerHealth.HealthChanged -= HandleHealthChanged;
            healthCallbacksRegistered = false;
        }

        private void StartGame()
        {
            mapChosen = false;
            SetStartPanelVisible(false);
            SetMapSelectVisible(true);
            SetSkinSelectVisible(false);
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
            SelectSkin(0);
            SetMapSelectVisible(false);
            SetSkinSelectVisible(true);
        }

        private void SelectPurpleSkin()
        {
            SelectSkin(0);
        }

        private void SelectGreenSkin()
        {
            SelectSkin(1);
        }

        private void SelectPinkSkin()
        {
            SelectSkin(2);
        }

        private void SelectYellowSkin()
        {
            SelectSkin(3);
        }

        private void SelectBeigeSkin()
        {
            SelectSkin(4);
        }

        private void SelectSkin(int index)
        {
            if (skinChoices == null || skinChoices.Length == 0) return;
            if (index < 0 || index >= skinChoices.Length) return;

            selectedSkin = skinChoices[index];

            if (skinPreviewImage != null)
            {
                skinPreviewImage.sprite = selectedSkin.Idle;
                skinPreviewImage.scaleMode = ScaleMode.ScaleToFit;
                skinPreviewImage.tintColor = Color.white;
            }

            if (skinNameLabel != null)
            {
                skinNameLabel.text = selectedSkin.Name;
            }

            for (int i = 0; i < skinChoices.Length; i++)
            {
                Button button = skinChoices[i].Button;
                if (button == null) continue;

                if (i == index)
                {
                    button.AddToClassList(SelectedSkinClass);
                }
                else
                {
                    button.RemoveFromClassList(SelectedSkinClass);
                }
            }
        }

        private void ConfirmSkinAndStart()
        {
            if (!mapChosen) return;

            if (mapBuilder != null)
            {
                mapBuilder.SelectMap(pendingMap);
                mapBuilder.BuildMap();
            }

            if (playerMovement != null)
            {
                playerMovement.ApplySkin(selectedSkin.Idle, selectedSkin.RunA, selectedSkin.RunB, selectedSkin.Jump, selectedSkin.Crouch);
                playerStartY = playerMovement.transform.position.y;
                lastSafeRespawnPosition = playerMovement.transform.position;
                lastSafeSampleTime = Time.time;
            }

            if (playerHealth != null)
            {
                playerHealth.ResetHealth(maxLives);
            }

            nextFallDamageTime = 0f;
            gameStarted = true;
            levelCompleted = false;
            levelFailed = false;
            SetSkinSelectVisible(false);
            SetSettingsButtonVisible(true);
            SetLivesVisible(true);
            UpdateLivesUI(playerHealth != null ? playerHealth.CurrentLives : maxLives, maxLives);
            SetGameplayInputEnabled(true);
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetFailureIconVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }
        }

        private void BackToMapSelect()
        {
            SetSkinSelectVisible(false);
            SetMapSelectVisible(true);
            SetSettingsVisible(false);
            SetRulesVisible(false);
        }

        private void ToggleSettings()
        {
            if (!gameStarted || levelCompleted || levelFailed || settingsPanel == null) return;

            bool isVisible = settingsPanel.style.display == DisplayStyle.Flex;
            SetSettingsVisible(!isVisible);
            if (isVisible)
            {
                SetRulesVisible(false);
            }
        }

        private void ShowRules()
        {
            if (!gameStarted || levelCompleted || levelFailed) return;

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
            SetFailureIconVisible(false);
            SetLivesVisible(false);

            if (resultLabel != null)
            {
                resultLabel.text = selectedMapTitle + " cleared!\nYou found your treasure.";
                resultLabel.style.display = DisplayStyle.Flex;
            }
        }

        private void CheckFallFailure()
        {
            if (!gameStarted || levelCompleted || levelFailed || playerMovement == null) return;
            if (Time.time < nextFallDamageTime) return;

            float fallBaselineY = Mathf.Max(playerStartY, lastSafeRespawnPosition.y);
            if (playerMovement.transform.position.y > fallBaselineY - fallFailureDistance) return;

            HandlePlayerFall();
        }

        private void UpdateSafeRespawnPosition()
        {
            if (!gameStarted || levelCompleted || levelFailed || playerMovement == null) return;
            if (!playerMovement.IsGrounded || playerMovement.transform.position.y <= playerStartY - 1f) return;
            if (Time.time - lastSafeSampleTime < safePositionSampleInterval) return;

            Vector3 position = playerMovement.transform.position;
            if (Vector2.Distance(position, lastSafeRespawnPosition) >= respawnBackDistance)
            {
                lastSafeRespawnPosition = position;
            }

            lastSafeSampleTime = Time.time;
        }

        private void HandlePlayerFall()
        {
            nextFallDamageTime = Time.time + respawnInvulnerabilityTime;

            if (playerHealth == null || !playerHealth.Damage(1) || playerHealth.IsDepleted)
            {
                HandleLevelFailed();
                return;
            }

            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            if (playerMovement == null) return;

            Vector3 respawnPosition = lastSafeRespawnPosition;
            playerMovement.transform.position = respawnPosition;

            if (playerBody != null)
            {
                playerBody.velocity = Vector2.zero;
                playerBody.angularVelocity = 0f;
            }
        }

        private void HandleLevelFailed()
        {
            levelFailed = true;
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetSettingsButtonVisible(false);
            SetLivesVisible(false);
            SetGameplayInputEnabled(false);
            SetFailureIconVisible(true);

            if (resultLabel != null)
            {
                resultLabel.text = "You fell!\nLevel failed.";
                resultLabel.style.display = DisplayStyle.Flex;
            }
        }

        private void HandleHealthChanged(int currentLives, int totalLives)
        {
            UpdateLivesUI(currentLives, totalLives);
        }

        private Image[] BuildLifeHeartElements(VisualElement root)
        {
            if (lifeHeartNames == null || lifeHeartNames.Length == 0) return new Image[0];

            Image[] hearts = new Image[lifeHeartNames.Length];
            for (int i = 0; i < lifeHeartNames.Length; i++)
            {
                hearts[i] = root.Q<Image>(lifeHeartNames[i]);
                DisableKeyboardFocus(hearts[i]);
            }

            return hearts;
        }

        private void UpdateLivesUI(int currentLives, int totalLives)
        {
            if (lifeHearts == null) return;

            int cappedTotal = Mathf.Clamp(totalLives, 0, lifeHearts.Length);
            for (int i = 0; i < lifeHearts.Length; i++)
            {
                Image heart = lifeHearts[i];
                if (heart == null) continue;

                heart.style.display = i < cappedTotal ? DisplayStyle.Flex : DisplayStyle.None;
                heart.sprite = i < currentLives ? fullHeartSprite : emptyHeartSprite;
                heart.scaleMode = ScaleMode.ScaleToFit;
                heart.tintColor = Color.white;
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

        private void SetSkinSelectVisible(bool visible)
        {
            if (skinSelectPanel != null)
            {
                skinSelectPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetSettingsButtonVisible(bool visible)
        {
            if (settingsButton != null)
            {
                settingsButton.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetLivesVisible(bool visible)
        {
            if (livesContainer != null)
            {
                livesContainer.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
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

        private void SetFailureIconVisible(bool visible)
        {
            if (failureIcon != null)
            {
                failureIcon.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
