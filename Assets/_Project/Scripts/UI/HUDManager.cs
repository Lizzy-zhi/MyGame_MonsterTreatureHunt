using UnityEngine;
using UnityEngine.UIElements;
using MonsterTreasureHunt.Gameplay;
using MonsterTreasureHunt.Levels;
using MonsterTreasureHunt.Player;
using MonsterTreasureHunt.CameraSystem;
using System;

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
        [SerializeField] private string levelPromptPanelName = "LevelPromptPanel";
        [SerializeField] private string levelPromptTitleName = "LevelPromptTitle";
        [SerializeField] private string levelPromptMessageName = "LevelPromptMessage";
        [SerializeField] private string levelPromptContinueButtonName = "LevelPromptContinueButton";
        [SerializeField] private string resultLabelName = "ResultLabel";
        [SerializeField] private string failureIconName = "FailureIcon";
        [SerializeField] private string livesContainerName = "LivesContainer";
        [SerializeField] private string[] lifeHeartNames = { "LifeHeart1", "LifeHeart2", "LifeHeart3" };
        [SerializeField] private string inventoryButtonName = "InventoryButton";
        [SerializeField] private string inventoryPanelName = "InventoryPanel";
        [SerializeField] private string inventoryYellowKeyRowName = "InventoryYellowKeyRow";
        [SerializeField] private string inventoryYellowKeyIconName = "InventoryYellowKeyIcon";
        [SerializeField] private string inventoryYellowKeyLabelName = "InventoryYellowKeyLabel";
        [SerializeField] private string inventoryRedKeyRowName = "InventoryRedKeyRow";
        [SerializeField] private string inventoryRedKeyIconName = "InventoryRedKeyIcon";
        [SerializeField] private string inventoryRedKeyLabelName = "InventoryRedKeyLabel";
        [SerializeField] private string inventoryGreenKeyRowName = "InventoryGreenKeyRow";
        [SerializeField] private string inventoryGreenKeyIconName = "InventoryGreenKeyIcon";
        [SerializeField] private string inventoryGreenKeyLabelName = "InventoryGreenKeyLabel";
        [SerializeField] private string inventoryBlueKeyRowName = "InventoryBlueKeyRow";
        [SerializeField] private string inventoryBlueKeyIconName = "InventoryBlueKeyIcon";
        [SerializeField] private string inventoryBlueKeyLabelName = "InventoryBlueKeyLabel";
        [SerializeField] private string chestLockedMessageName = "ChestLockedMessage";
        [SerializeField] private string gameplayHintMessageName = "GameplayHintMessage";
        [SerializeField] private string victoryPanelName = "VictoryPanel";
        [SerializeField] private string victoryMessageName = "VictoryMessage";
        [SerializeField] private string victoryRewardName = "VictoryReward";

        [Header("Level")]
        [SerializeField] private IslandLevelController levelController;
        [SerializeField] private IslandMapBuilder mapBuilder;
        [SerializeField] private CameraFollow2D cameraFollow;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private TreasureCollectible treasure;
        [SerializeField] private float fallFailureDistance = 12f;
        [SerializeField] private int maxLives = 3;
        [SerializeField] private float respawnBackDistance = 4f;
        [SerializeField] private float safePositionSampleInterval = 0.25f;
        [SerializeField] private float respawnInvulnerabilityTime = 0.75f;

        [Header("Lives HUD")]
        [SerializeField] private Sprite fullHeartSprite;
        [SerializeField] private Sprite emptyHeartSprite;
        [SerializeField] private Sprite yellowInventoryKeySprite;
        [SerializeField] private Sprite redInventoryKeySprite;
        [SerializeField] private Sprite greenInventoryKeySprite;
        [SerializeField] private Sprite blueInventoryKeySprite;

        [Header("Purple Skin")]
        [SerializeField] private Sprite purpleIdleSprite;
        [SerializeField] private Sprite purpleRunSpriteA;
        [SerializeField] private Sprite purpleRunSpriteB;
        [SerializeField] private Sprite purpleJumpSprite;
        [SerializeField] private Sprite purpleCrouchSprite;
        [SerializeField] private Sprite purpleClimbSpriteA;
        [SerializeField] private Sprite purpleClimbSpriteB;

        [Header("Green Skin")]
        [SerializeField] private Sprite greenIdleSprite;
        [SerializeField] private Sprite greenRunSpriteA;
        [SerializeField] private Sprite greenRunSpriteB;
        [SerializeField] private Sprite greenJumpSprite;
        [SerializeField] private Sprite greenCrouchSprite;
        [SerializeField] private Sprite greenClimbSpriteA;
        [SerializeField] private Sprite greenClimbSpriteB;

        [Header("Pink Skin")]
        [SerializeField] private Sprite pinkIdleSprite;
        [SerializeField] private Sprite pinkRunSpriteA;
        [SerializeField] private Sprite pinkRunSpriteB;
        [SerializeField] private Sprite pinkJumpSprite;
        [SerializeField] private Sprite pinkCrouchSprite;
        [SerializeField] private Sprite pinkClimbSpriteA;
        [SerializeField] private Sprite pinkClimbSpriteB;

        [Header("Yellow Skin")]
        [SerializeField] private Sprite yellowIdleSprite;
        [SerializeField] private Sprite yellowRunSpriteA;
        [SerializeField] private Sprite yellowRunSpriteB;
        [SerializeField] private Sprite yellowJumpSprite;
        [SerializeField] private Sprite yellowCrouchSprite;
        [SerializeField] private Sprite yellowClimbSpriteA;
        [SerializeField] private Sprite yellowClimbSpriteB;

        [Header("Beige Skin")]
        [SerializeField] private Sprite beigeIdleSprite;
        [SerializeField] private Sprite beigeRunSpriteA;
        [SerializeField] private Sprite beigeRunSpriteB;
        [SerializeField] private Sprite beigeJumpSprite;
        [SerializeField] private Sprite beigeCrouchSprite;
        [SerializeField] private Sprite beigeClimbSpriteA;
        [SerializeField] private Sprite beigeClimbSpriteB;

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
        private VisualElement levelPromptPanel;
        private Label levelPromptTitle;
        private Label levelPromptMessage;
        private Button levelPromptContinueButton;
        private Label resultLabel;
        private Label failureIcon;
        private VisualElement livesContainer;
        private Image[] lifeHearts;
        private Button inventoryButton;
        private VisualElement inventoryPanel;
        private VisualElement inventoryYellowKeyRow;
        private Image inventoryYellowKeyIcon;
        private Label inventoryYellowKeyLabel;
        private Label inventoryYellowKeyCount;
        private VisualElement inventoryRedKeyRow;
        private Image inventoryRedKeyIcon;
        private Label inventoryRedKeyLabel;
        private Label inventoryRedKeyCount;
        private VisualElement inventoryGreenKeyRow;
        private Image inventoryGreenKeyIcon;
        private Label inventoryGreenKeyLabel;
        private Label inventoryGreenKeyCount;
        private VisualElement inventoryBlueKeyRow;
        private Image inventoryBlueKeyIcon;
        private Label inventoryBlueKeyLabel;
        private Label inventoryBlueKeyCount;
        private Label chestLockedMessage;
        private Label gameplayHintMessage;
        private VisualElement victoryPanel;
        private Label victoryMessage;
        private Label victoryReward;

        private bool levelCompleted;
        private bool levelFailed;
        private bool gameStarted;
        private Rigidbody2D playerBody;
        private float playerStartY;
        private Vector3 lastSafeRespawnPosition;
        private float lastSafeSampleTime;
        private float nextFallDamageTime;
        private bool healthCallbacksRegistered;
        private bool inventoryCallbacksRegistered;
        private bool treasureCallbacksRegistered;
        private bool preLevelPromptActive;
        private TreasureCollectible[] observedTreasures = Array.Empty<TreasureCollectible>();
        private float hideLockedMessageAt;
        private float hideGameplayHintAt;
        private string activeGameplayHintText = string.Empty;
        private IslandMapBuilder.MapTheme pendingMap = IslandMapBuilder.MapTheme.BeginnerIsland;
        private bool mapChosen;
        private string selectedMapTitle = "Beginner Island";
        private SkinChoice selectedSkin;
        private SkinChoice[] skinChoices;

        private const string SelectedSkinClass = "skin-option-selected";
        private const string RulesText =
            "Guide your little monster across each island and unlock every treasure chest to win.\n" +
            "Move with A / D or the arrow keys, and press Space to jump between platforms.\n" +
            "Use Up / Down on ladders to reach high platforms.\n" +
            "Hold S or the Down Arrow to crouch, and keep moving to shuffle through low passages.\n" +
            "Collect keys and use the matching color to open each chest.\n" +
            "You have three lives. Falling costs one life and sends you back to the last safe ground.\n" +
            "Press I at any time to check the keys collected in the current run.";
        private const string BeginnerPromptTitle = "Beginner Guide";
        private const string BeginnerPromptText =
            "Use A / D or the arrow keys to move.\n" +
            "Press Space to jump between platforms.\n" +
            "Hold S or the Down Arrow to crouch and shuffle through low gaps.\n" +
            "Find the yellow key, then open the yellow chest to clear the island.";
        private const string FoggyPromptTitle = "Foggy Forest Briefing";
        private const string FoggyPromptText =
            "Foggy Forest uses multiple colored keys and chests.\n" +
            "Watch for the flying bee on the trail - touching it costs one life and knocks you back.\n" +
            "Jumping fish can burst out of the river between platforms, so time your crossings carefully.\n" +
            "Explore both levels of the route, collect the yellow, red, and green keys,\n" +
            "and open the matching chests in a safe order.\n" +
            "Press I if you need to check which keys you already have.";
        private const string VolcanoPromptTitle = "Volcano Warning";
        private const string VolcanoPromptText =
            "Spikes and fire slimes in Volcano Cave both deal damage and knock your monster backward.\n" +
            "A fake green chest will turn into a monster and explode after three seconds if you get too close.\n" +
            "Use ladders with Up / Down to reach high platforms, then keep space before each landing across the lava.";

        private struct SkinChoice
        {
            public string Name;
            public Sprite Idle;
            public Sprite RunA;
            public Sprite RunB;
            public Sprite Jump;
            public Sprite Crouch;
            public Sprite ClimbA;
            public Sprite ClimbB;
            public Button Button;

            public SkinChoice(string name, Sprite idle, Sprite runA, Sprite runB, Sprite jump, Sprite crouch, Sprite climbA, Sprite climbB, Button button)
            {
                Name = name;
                Idle = idle;
                RunA = runA;
                RunB = runB;
                Jump = jump;
                Crouch = crouch;
                ClimbA = climbA;
                ClimbB = climbB;
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
            levelPromptPanel = root.Q<VisualElement>(levelPromptPanelName);
            levelPromptTitle = root.Q<Label>(levelPromptTitleName);
            levelPromptMessage = root.Q<Label>(levelPromptMessageName);
            levelPromptContinueButton = root.Q<Button>(levelPromptContinueButtonName);
            resultLabel = root.Q<Label>(resultLabelName);
            failureIcon = root.Q<Label>(failureIconName);
            livesContainer = root.Q<VisualElement>(livesContainerName);
            lifeHearts = BuildLifeHeartElements(root);
            inventoryButton = root.Q<Button>(inventoryButtonName);
            inventoryPanel = root.Q<VisualElement>(inventoryPanelName);
            inventoryYellowKeyRow = root.Q<VisualElement>(inventoryYellowKeyRowName);
            inventoryYellowKeyIcon = root.Q<Image>(inventoryYellowKeyIconName);
            inventoryYellowKeyLabel = root.Q<Label>(inventoryYellowKeyLabelName);
            inventoryYellowKeyCount = root.Q<Label>("InventoryYellowKeyCount");
            inventoryRedKeyRow = root.Q<VisualElement>(inventoryRedKeyRowName);
            inventoryRedKeyIcon = root.Q<Image>(inventoryRedKeyIconName);
            inventoryRedKeyLabel = root.Q<Label>(inventoryRedKeyLabelName);
            inventoryRedKeyCount = root.Q<Label>("InventoryRedKeyCount");
            inventoryGreenKeyRow = root.Q<VisualElement>(inventoryGreenKeyRowName);
            inventoryGreenKeyIcon = root.Q<Image>(inventoryGreenKeyIconName);
            inventoryGreenKeyLabel = root.Q<Label>(inventoryGreenKeyLabelName);
            inventoryGreenKeyCount = root.Q<Label>("InventoryGreenKeyCount");
            inventoryBlueKeyRow = root.Q<VisualElement>(inventoryBlueKeyRowName);
            inventoryBlueKeyIcon = root.Q<Image>(inventoryBlueKeyIconName);
            inventoryBlueKeyLabel = root.Q<Label>(inventoryBlueKeyLabelName);
            inventoryBlueKeyCount = root.Q<Label>("InventoryBlueKeyCount");
            chestLockedMessage = root.Q<Label>(chestLockedMessageName);
            gameplayHintMessage = root.Q<Label>(gameplayHintMessageName);
            victoryPanel = root.Q<VisualElement>(victoryPanelName);
            victoryMessage = root.Q<Label>(victoryMessageName);
            victoryReward = root.Q<Label>(victoryRewardName);

            DisableKeyboardFocus(settingsButton);
            DisableKeyboardFocus(helpButton);
            DisableKeyboardFocus(continueButton);
            DisableKeyboardFocus(escapeButton);
            DisableKeyboardFocus(startButton);
            DisableKeyboardFocus(startQuitButton);
            DisableKeyboardFocus(beginnerMapButton);
            DisableKeyboardFocus(foggyMapButton);
            DisableKeyboardFocus(volcanoMapButton);
            DisableKeyboardFocus(levelPromptContinueButton);
            DisableKeyboardFocus(purpleSkinButton);
            DisableKeyboardFocus(greenSkinButton);
            DisableKeyboardFocus(pinkSkinButton);
            DisableKeyboardFocus(yellowSkinButton);
            DisableKeyboardFocus(beigeSkinButton);
            DisableKeyboardFocus(confirmSkinButton);
            DisableKeyboardFocus(backToMapButton);
            DisableKeyboardFocus(inventoryButton);
            root.Focus();

            if (mapBuilder == null)
            {
                mapBuilder = FindObjectOfType<IslandMapBuilder>();
            }

            if (cameraFollow == null)
            {
                cameraFollow = FindObjectOfType<CameraFollow2D>();
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

                if (playerInventory == null)
                {
                    playerInventory = playerMovement.GetComponent<PlayerInventory>();
                    if (playerInventory == null)
                    {
                        playerInventory = playerMovement.gameObject.AddComponent<PlayerInventory>();
                    }
                }
            }

            if (playerHealth == null)
            {
                playerHealth = FindObjectOfType<PlayerHealth>();
            }

            if (playerInventory == null)
            {
                playerInventory = FindObjectOfType<PlayerInventory>();
            }

            if (treasure == null)
            {
                treasure = FindObjectOfType<TreasureCollectible>();
            }

            BuildSkinChoices();
            RegisterCallbacks();
            ConfigureInventoryIcons();

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
            SetInventoryButtonVisible(false);
            SetInventoryVisible(false);
            SetGameplayInputEnabled(false);
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetLevelPromptVisible(false);
            SetVictoryVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }

            SetFailureIconVisible(false);
            SetChestLockedMessageVisible(false);
            SetGameplayHintVisible(false);
        }

        private void Update()
        {
            if (gameStarted && !levelCompleted && !levelFailed && !preLevelPromptActive && Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }

            if (chestLockedMessage != null &&
                chestLockedMessage.style.display == DisplayStyle.Flex &&
                hideLockedMessageAt > 0f &&
                Time.time >= hideLockedMessageAt)
            {
                SetChestLockedMessageVisible(false);
            }

            if (gameplayHintMessage != null &&
                gameplayHintMessage.style.display == DisplayStyle.Flex &&
                hideGameplayHintAt > 0f &&
                Time.time >= hideGameplayHintAt)
            {
                HideGameplayHint(activeGameplayHintText);
            }

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
                new SkinChoice("Purple", purpleIdleSprite, purpleRunSpriteA, purpleRunSpriteB, purpleJumpSprite, purpleCrouchSprite, purpleClimbSpriteA, purpleClimbSpriteB, purpleSkinButton),
                new SkinChoice("Green", greenIdleSprite, greenRunSpriteA, greenRunSpriteB, greenJumpSprite, greenCrouchSprite, greenClimbSpriteA, greenClimbSpriteB, greenSkinButton),
                new SkinChoice("Pink", pinkIdleSprite, pinkRunSpriteA, pinkRunSpriteB, pinkJumpSprite, pinkCrouchSprite, pinkClimbSpriteA, pinkClimbSpriteB, pinkSkinButton),
                new SkinChoice("Yellow", yellowIdleSprite, yellowRunSpriteA, yellowRunSpriteB, yellowJumpSprite, yellowCrouchSprite, yellowClimbSpriteA, yellowClimbSpriteB, yellowSkinButton),
                new SkinChoice("Beige", beigeIdleSprite, beigeRunSpriteA, beigeRunSpriteB, beigeJumpSprite, beigeCrouchSprite, beigeClimbSpriteA, beigeClimbSpriteB, beigeSkinButton),
            };
        }

        private void RegisterCallbacks()
        {
            if (startButton != null) startButton.clicked += StartGame;
            if (startQuitButton != null) startQuitButton.clicked += EscapeGame;
            if (beginnerMapButton != null) beginnerMapButton.clicked += SelectBeginnerMap;
            if (foggyMapButton != null) foggyMapButton.clicked += SelectFoggyMap;
            if (volcanoMapButton != null) volcanoMapButton.clicked += SelectVolcanoMap;
            if (levelPromptContinueButton != null) levelPromptContinueButton.clicked += DismissLevelPrompt;
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
            if (inventoryButton != null) inventoryButton.clicked += ToggleInventory;
            RegisterHealthCallbacks();
            RegisterInventoryCallbacks();
            RegisterTreasureCallbacks();
        }

        private void UnregisterCallbacks()
        {
            if (startButton != null) startButton.clicked -= StartGame;
            if (startQuitButton != null) startQuitButton.clicked -= EscapeGame;
            if (beginnerMapButton != null) beginnerMapButton.clicked -= SelectBeginnerMap;
            if (foggyMapButton != null) foggyMapButton.clicked -= SelectFoggyMap;
            if (volcanoMapButton != null) volcanoMapButton.clicked -= SelectVolcanoMap;
            if (levelPromptContinueButton != null) levelPromptContinueButton.clicked -= DismissLevelPrompt;
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
            if (inventoryButton != null) inventoryButton.clicked -= ToggleInventory;
            UnregisterHealthCallbacks();
            UnregisterInventoryCallbacks();
            UnregisterTreasureCallbacks();
        }

        private void RegisterHealthCallbacks()
        {
            if (playerHealth == null || healthCallbacksRegistered) return;

            playerHealth.HealthChanged += HandleHealthChanged;
            playerHealth.Damaged += HandlePlayerDamaged;
            healthCallbacksRegistered = true;
        }

        private void UnregisterHealthCallbacks()
        {
            if (playerHealth == null || !healthCallbacksRegistered) return;

            playerHealth.HealthChanged -= HandleHealthChanged;
            playerHealth.Damaged -= HandlePlayerDamaged;
            healthCallbacksRegistered = false;
        }

        private void RegisterInventoryCallbacks()
        {
            if (playerInventory == null || inventoryCallbacksRegistered) return;

            playerInventory.InventoryChanged += HandleInventoryChanged;
            inventoryCallbacksRegistered = true;
        }

        private void UnregisterInventoryCallbacks()
        {
            if (playerInventory == null || !inventoryCallbacksRegistered) return;

            playerInventory.InventoryChanged -= HandleInventoryChanged;
            inventoryCallbacksRegistered = false;
        }

        private void RegisterTreasureCallbacks()
        {
            if (treasureCallbacksRegistered) return;

            observedTreasures = FindObjectsOfType<TreasureCollectible>(false);
            if ((observedTreasures == null || observedTreasures.Length == 0) && treasure != null)
            {
                observedTreasures = new[] { treasure };
            }

            if (observedTreasures == null || observedTreasures.Length == 0) return;

            for (int i = 0; i < observedTreasures.Length; i++)
            {
                if (observedTreasures[i] != null)
                {
                    observedTreasures[i].Locked += HandleTreasureLocked;
                }
            }

            treasureCallbacksRegistered = true;
        }

        private void UnregisterTreasureCallbacks()
        {
            if (!treasureCallbacksRegistered) return;

            if (observedTreasures != null)
            {
                for (int i = 0; i < observedTreasures.Length; i++)
                {
                    if (observedTreasures[i] != null)
                    {
                        observedTreasures[i].Locked -= HandleTreasureLocked;
                    }
                }
            }

            observedTreasures = Array.Empty<TreasureCollectible>();
            treasureCallbacksRegistered = false;
        }

        private void RefreshTreasureCallbacks()
        {
            UnregisterTreasureCallbacks();
            treasure = FindObjectOfType<TreasureCollectible>();
            RegisterTreasureCallbacks();
        }

        private void StartGame()
        {
            mapChosen = false;
            SetStartPanelVisible(false);
            SetMapSelectVisible(true);
            SetSkinSelectVisible(false);
            SetSettingsButtonVisible(false);
            SetInventoryButtonVisible(false);
            SetInventoryVisible(false);
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetLevelPromptVisible(false);
            SetVictoryVisible(false);
            SetGameplayHintVisible(false);
        }

        private void SelectBeginnerMap()
        {
            SelectMap(IslandMapBuilder.MapTheme.BeginnerIsland, "Beginner Island");
        }

        private void SelectFoggyMap()
        {
            SelectMap(IslandMapBuilder.MapTheme.FoggyForest, "Foggy Forest");
        }

        private void SelectVolcanoMap()
        {
            SelectMap(IslandMapBuilder.MapTheme.VolcanoCave, "Volcano Cave");
        }

        private void SelectMap(IslandMapBuilder.MapTheme map, string mapTitle)
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
                UnregisterTreasureCallbacks();
                mapBuilder.SelectMap(pendingMap);
                mapBuilder.BuildMap();
            }

            if (cameraFollow != null)
            {
                cameraFollow.ApplyMapTheme(pendingMap);
            }

            if (levelController != null)
            {
                levelController.ResetLevel();
            }

            if (playerMovement != null)
            {
                playerMovement.ApplySkin(selectedSkin.Idle, selectedSkin.RunA, selectedSkin.RunB, selectedSkin.Jump, selectedSkin.Crouch, selectedSkin.ClimbA, selectedSkin.ClimbB);
                playerStartY = playerMovement.transform.position.y;
                lastSafeRespawnPosition = playerMovement.transform.position;
                lastSafeSampleTime = Time.time;
            }

            if (playerHealth != null)
            {
                playerHealth.ResetHealth(maxLives);
            }

            if (playerInventory != null)
            {
                playerInventory.ResetInventory();
                UpdateInventoryUI(playerInventory);
            }

            RefreshTreasureCallbacks();

            nextFallDamageTime = 0f;
            hideLockedMessageAt = 0f;
            hideGameplayHintAt = 0f;
            activeGameplayHintText = string.Empty;
            gameStarted = true;
            levelCompleted = false;
            levelFailed = false;
            preLevelPromptActive = false;
            SetSkinSelectVisible(false);
            SetSettingsButtonVisible(false);
            SetInventoryButtonVisible(false);
            SetInventoryVisible(false);
            SetLivesVisible(false);
            UpdateLivesUI(playerHealth != null ? playerHealth.CurrentLives : maxLives, maxLives);
            SetGameplayInputEnabled(false);
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetLevelPromptVisible(false);
            SetFailureIconVisible(false);
            SetVictoryVisible(false);
            SetChestLockedMessageVisible(false);
            SetGameplayHintVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }

            if (pendingMap == IslandMapBuilder.MapTheme.BeginnerIsland)
            {
                ShowLevelPrompt(BeginnerPromptTitle, BeginnerPromptText);
                return;
            }

            if (pendingMap == IslandMapBuilder.MapTheme.FoggyForest)
            {
                ShowLevelPrompt(FoggyPromptTitle, FoggyPromptText);
                return;
            }

            if (pendingMap == IslandMapBuilder.MapTheme.VolcanoCave)
            {
                ShowLevelPrompt(VolcanoPromptTitle, VolcanoPromptText);
                return;
            }

            FinalizeGameplayStart();
        }

        private void BackToMapSelect()
        {
            preLevelPromptActive = false;
            SetSkinSelectVisible(false);
            SetMapSelectVisible(true);
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetLevelPromptVisible(false);
            SetInventoryVisible(false);
            SetVictoryVisible(false);
            SetGameplayHintVisible(false);
        }

        private void ToggleSettings()
        {
            if (!gameStarted || levelCompleted || levelFailed || settingsPanel == null) return;
            if (preLevelPromptActive) return;

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
            if (preLevelPromptActive) return;

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
            preLevelPromptActive = false;
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetLevelPromptVisible(false);
            SetFailureIconVisible(false);
            SetLivesVisible(false);
            SetInventoryButtonVisible(false);
            SetInventoryVisible(false);
            SetGameplayInputEnabled(false);
            SetVictoryVisible(true);
            SetGameplayHintVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
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

            if (playerHealth == null)
            {
                HandleLevelFailed();
                return;
            }

            if (!playerHealth.Damage(1, PlayerHealth.DamageSource.Fall))
            {
                return;
            }

            if (playerHealth.IsDepleted)
            {
                HandleLevelFailed();
                return;
            }

            RespawnPlayer();
        }

        private void HandlePlayerDamaged(PlayerHealth.DamageSource source)
        {
            if (!gameStarted || levelCompleted || levelFailed) return;
            if (source != PlayerHealth.DamageSource.Spike &&
                source != PlayerHealth.DamageSource.Bee &&
                source != PlayerHealth.DamageSource.FireSlime &&
                source != PlayerHealth.DamageSource.FakeChestExplosion &&
                source != PlayerHealth.DamageSource.Fish) return;

            nextFallDamageTime = Time.time + respawnInvulnerabilityTime;

            if (playerHealth != null && playerHealth.IsDepleted) return;

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
            SetInventoryButtonVisible(false);
            SetInventoryVisible(false);
            SetGameplayInputEnabled(false);
            SetFailureIconVisible(true);
            SetGameplayHintVisible(false);

            if (resultLabel != null)
            {
                resultLabel.text = GetFailureMessage();
                resultLabel.style.display = DisplayStyle.Flex;
            }
        }

        private string GetFailureMessage()
        {
            if (playerHealth != null && playerHealth.LastDamageSource == PlayerHealth.DamageSource.Bee)
            {
                return "Bitten by the bee!\nOut of lives.";
            }

            if (playerHealth != null && playerHealth.LastDamageSource == PlayerHealth.DamageSource.FireSlime)
            {
                return "Burned by the fire slime!\nOut of lives.";
            }

            if (playerHealth != null && playerHealth.LastDamageSource == PlayerHealth.DamageSource.Fish)
            {
                return "Hit by the jumping fish!\nOut of lives.";
            }

            if (playerHealth != null && playerHealth.LastDamageSource == PlayerHealth.DamageSource.FakeChestExplosion)
            {
                return "Caught in the fake chest blast!\nOut of lives.";
            }

            if (playerHealth != null && playerHealth.LastDamageSource == PlayerHealth.DamageSource.Spike)
            {
                return "Impaled by spikes!\nOut of lives.";
            }

            return "You fell too far!\nOut of lives.";
        }

        private void HandleHealthChanged(int currentLives, int totalLives)
        {
            UpdateLivesUI(currentLives, totalLives);

            if (gameStarted && !levelCompleted && !levelFailed && currentLives <= 0)
            {
                HandleLevelFailed();
            }
        }

        private void HandleInventoryChanged(PlayerInventory inventory)
        {
            UpdateInventoryUI(inventory);
        }

        private void HandleTreasureLocked(TreasureCollectible lockedTreasure)
        {
            if (!gameStarted || levelCompleted || levelFailed) return;

            if (chestLockedMessage != null)
            {
                string colorName = TreasureKeyColorUtility.GetDisplayName(lockedTreasure.RequiredKeyColor);
                chestLockedMessage.text = $"{colorName} Key Required";
            }

            SetChestLockedMessageVisible(true);
            hideLockedMessageAt = Time.time + 2.5f;
        }

        private void ConfigureInventoryIcons()
        {
            ConfigureInventoryIcon(inventoryYellowKeyIcon, yellowInventoryKeySprite);
            ConfigureInventoryIcon(inventoryRedKeyIcon, redInventoryKeySprite);
            ConfigureInventoryIcon(inventoryGreenKeyIcon, greenInventoryKeySprite);
            ConfigureInventoryIcon(inventoryBlueKeyIcon, blueInventoryKeySprite);
        }

        private void UpdateInventoryUI(PlayerInventory inventory)
        {
            UpdateInventoryRow(inventoryYellowKeyRow, inventoryYellowKeyLabel, inventoryYellowKeyCount, "Yellow Key", GetKeyCount(inventory, TreasureKeyColor.Yellow), true);
            UpdateInventoryRow(inventoryRedKeyRow, inventoryRedKeyLabel, inventoryRedKeyCount, "Red Key", GetKeyCount(inventory, TreasureKeyColor.Red), MapUsesColoredKeys(pendingMap));
            UpdateInventoryRow(inventoryGreenKeyRow, inventoryGreenKeyLabel, inventoryGreenKeyCount, "Green Key", GetKeyCount(inventory, TreasureKeyColor.Green), MapUsesColoredKeys(pendingMap));
            UpdateInventoryRow(inventoryBlueKeyRow, inventoryBlueKeyLabel, inventoryBlueKeyCount, "Blue Key", GetKeyCount(inventory, TreasureKeyColor.Blue), false);
        }

        private static void ConfigureInventoryIcon(Image icon, Sprite sprite)
        {
            if (icon == null) return;

            icon.sprite = sprite;
            icon.scaleMode = ScaleMode.ScaleToFit;
            icon.tintColor = Color.white;
        }

        private static int GetKeyCount(PlayerInventory inventory, TreasureKeyColor color)
        {
            return inventory != null ? inventory.GetKeyCount(color) : 0;
        }

        private static bool MapUsesColoredKeys(IslandMapBuilder.MapTheme map)
        {
            return map == IslandMapBuilder.MapTheme.FoggyForest
                || map == IslandMapBuilder.MapTheme.VolcanoCave;
        }

        private static void UpdateInventoryRow(VisualElement row, Label label, Label countLabel, string itemName, int count, bool visible)
        {
            if (row != null)
            {
                row.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (label != null)
            {
                label.text = itemName;
            }

            if (countLabel != null)
            {
                countLabel.text = $"x{count}";
            }
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

        private void SetInventoryButtonVisible(bool visible)
        {
            if (inventoryButton != null)
            {
                inventoryButton.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void ToggleInventory()
        {
            if (!gameStarted || levelCompleted || levelFailed || preLevelPromptActive || inventoryPanel == null) return;

            bool isVisible = inventoryPanel.style.display == DisplayStyle.Flex;
            SetInventoryVisible(!isVisible);
        }

        private void SetInventoryVisible(bool visible)
        {
            if (inventoryPanel != null)
            {
                inventoryPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
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

        private void SetLevelPromptVisible(bool visible)
        {
            if (levelPromptPanel != null)
            {
                levelPromptPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetFailureIconVisible(bool visible)
        {
            if (failureIcon != null)
            {
                failureIcon.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetChestLockedMessageVisible(bool visible)
        {
            if (chestLockedMessage != null)
            {
                chestLockedMessage.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void SetGameplayHintVisible(bool visible)
        {
            if (gameplayHintMessage != null)
            {
                gameplayHintMessage.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (!visible)
            {
                hideGameplayHintAt = 0f;
                activeGameplayHintText = string.Empty;
            }
        }

        private void SetVictoryVisible(bool visible)
        {
            if (victoryPanel != null)
            {
                victoryPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (!visible) return;

            if (victoryMessage != null)
            {
                victoryMessage.text = $"You unlocked every chest and cleared {selectedMapTitle}.";
            }

            if (victoryReward != null)
            {
                victoryReward.text = "Victory";
            }
        }

        private void ShowLevelPrompt(string title, string message)
        {
            preLevelPromptActive = true;

            if (levelPromptTitle != null)
            {
                levelPromptTitle.text = title;
            }

            if (levelPromptMessage != null)
            {
                levelPromptMessage.text = message;
            }

            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetInventoryVisible(false);
            SetGameplayHintVisible(false);
            SetLevelPromptVisible(true);
        }

        private void DismissLevelPrompt()
        {
            if (!preLevelPromptActive) return;

            preLevelPromptActive = false;
            SetLevelPromptVisible(false);
            FinalizeGameplayStart();
        }

        private void FinalizeGameplayStart()
        {
            SetSettingsButtonVisible(true);
            SetInventoryButtonVisible(true);
            SetLivesVisible(true);
            SetGameplayInputEnabled(true);
        }

        public void ShowGameplayHint(string message, float autoHideDelay = 0.2f)
        {
            if (!gameStarted || levelCompleted || levelFailed || preLevelPromptActive) return;
            if (string.IsNullOrWhiteSpace(message) || gameplayHintMessage == null) return;

            activeGameplayHintText = message;
            gameplayHintMessage.text = message;
            gameplayHintMessage.style.display = DisplayStyle.Flex;
            hideGameplayHintAt = autoHideDelay > 0f ? Time.time + autoHideDelay : 0f;
        }

        public void HideGameplayHint(string message = null)
        {
            if (gameplayHintMessage == null) return;
            if (!string.IsNullOrEmpty(message) &&
                !string.Equals(activeGameplayHintText, message, StringComparison.Ordinal))
            {
                return;
            }

            SetGameplayHintVisible(false);
        }
    }
}
