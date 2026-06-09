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
        private Button inventoryButton;
        private VisualElement inventoryPanel;
        private VisualElement inventoryYellowKeyRow;
        private Image inventoryYellowKeyIcon;
        private Label inventoryYellowKeyLabel;
        private VisualElement inventoryRedKeyRow;
        private Image inventoryRedKeyIcon;
        private Label inventoryRedKeyLabel;
        private VisualElement inventoryGreenKeyRow;
        private Image inventoryGreenKeyIcon;
        private Label inventoryGreenKeyLabel;
        private VisualElement inventoryBlueKeyRow;
        private Image inventoryBlueKeyIcon;
        private Label inventoryBlueKeyLabel;
        private Label chestLockedMessage;
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
        private TreasureCollectible[] observedTreasures = Array.Empty<TreasureCollectible>();
        private float hideLockedMessageAt;
        private IslandMapBuilder.MapTheme pendingMap = IslandMapBuilder.MapTheme.BeginnerIsland;
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
            "Find matching colored keys before opening colored treasure chests.\n" +
            "Press I to view the items collected this round.\n" +
            "Unlock every treasure chest to clear the island.";

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
            inventoryButton = root.Q<Button>(inventoryButtonName);
            inventoryPanel = root.Q<VisualElement>(inventoryPanelName);
            inventoryYellowKeyRow = root.Q<VisualElement>(inventoryYellowKeyRowName);
            inventoryYellowKeyIcon = root.Q<Image>(inventoryYellowKeyIconName);
            inventoryYellowKeyLabel = root.Q<Label>(inventoryYellowKeyLabelName);
            inventoryRedKeyRow = root.Q<VisualElement>(inventoryRedKeyRowName);
            inventoryRedKeyIcon = root.Q<Image>(inventoryRedKeyIconName);
            inventoryRedKeyLabel = root.Q<Label>(inventoryRedKeyLabelName);
            inventoryGreenKeyRow = root.Q<VisualElement>(inventoryGreenKeyRowName);
            inventoryGreenKeyIcon = root.Q<Image>(inventoryGreenKeyIconName);
            inventoryGreenKeyLabel = root.Q<Label>(inventoryGreenKeyLabelName);
            inventoryBlueKeyRow = root.Q<VisualElement>(inventoryBlueKeyRowName);
            inventoryBlueKeyIcon = root.Q<Image>(inventoryBlueKeyIconName);
            inventoryBlueKeyLabel = root.Q<Label>(inventoryBlueKeyLabelName);
            chestLockedMessage = root.Q<Label>(chestLockedMessageName);
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
            SetVictoryVisible(false);

            if (resultLabel != null)
            {
                resultLabel.style.display = DisplayStyle.None;
                resultLabel.text = string.Empty;
            }

            SetFailureIconVisible(false);
            SetChestLockedMessageVisible(false);
        }

        private void Update()
        {
            if (gameStarted && !levelCompleted && !levelFailed && Input.GetKeyDown(KeyCode.I))
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
            SetVictoryVisible(false);
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
                playerMovement.ApplySkin(selectedSkin.Idle, selectedSkin.RunA, selectedSkin.RunB, selectedSkin.Jump, selectedSkin.Crouch);
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
            gameStarted = true;
            levelCompleted = false;
            levelFailed = false;
            SetSkinSelectVisible(false);
            SetSettingsButtonVisible(true);
            SetInventoryButtonVisible(true);
            SetInventoryVisible(false);
            SetLivesVisible(true);
            UpdateLivesUI(playerHealth != null ? playerHealth.CurrentLives : maxLives, maxLives);
            SetGameplayInputEnabled(true);
            SetSettingsVisible(false);
            SetRulesVisible(false);
            SetFailureIconVisible(false);
            SetVictoryVisible(false);
            SetChestLockedMessageVisible(false);

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
            SetInventoryVisible(false);
            SetVictoryVisible(false);
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
            SetInventoryButtonVisible(false);
            SetInventoryVisible(false);
            SetGameplayInputEnabled(false);
            SetVictoryVisible(true);

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
            if (source != PlayerHealth.DamageSource.Hazard) return;

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

            if (resultLabel != null)
            {
                resultLabel.text = GetFailureMessage();
                resultLabel.style.display = DisplayStyle.Flex;
            }
        }

        private string GetFailureMessage()
        {
            if (playerHealth != null && playerHealth.LastDamageSource == PlayerHealth.DamageSource.Hazard)
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
            UpdateInventoryRow(inventoryYellowKeyRow, inventoryYellowKeyLabel, "Yellow Key", GetKeyCount(inventory, TreasureKeyColor.Yellow), true);
            UpdateInventoryRow(inventoryRedKeyRow, inventoryRedKeyLabel, "Red Key", GetKeyCount(inventory, TreasureKeyColor.Red), MapUsesColoredKeys(pendingMap));
            UpdateInventoryRow(inventoryGreenKeyRow, inventoryGreenKeyLabel, "Green Key", GetKeyCount(inventory, TreasureKeyColor.Green), MapUsesColoredKeys(pendingMap));
            UpdateInventoryRow(inventoryBlueKeyRow, inventoryBlueKeyLabel, "Blue Key", GetKeyCount(inventory, TreasureKeyColor.Blue), false);
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

        private static void UpdateInventoryRow(VisualElement row, Label label, string itemName, int count, bool visible)
        {
            if (row != null)
            {
                row.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (label != null)
            {
                label.text = $"{itemName}  x{count}";
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
            if (!gameStarted || levelCompleted || levelFailed || inventoryPanel == null) return;

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
    }
}
