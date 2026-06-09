# Monster Treasure Hunt Script Overview

This folder is organized by runtime responsibility:

- `Camera/`: camera follow and parallax background behavior.
- `Gameplay/`: level rules, map generation, pickups, health, inventory, and treasure collection.
- `Player/`: player movement and animation state.
- `Scent/`: treasure scent arrow UI behavior.
- `UI/`: game flow, HUD, settings, inventory, lives, map/skin selection, and result panels.
- `Editor/`: Unity Editor-only tools for building maps from the Inspector.

## Camera

### `Camera/CameraFollow2D.cs`
Main 2D camera controller. Follows the player smoothly and builds scrolling parallax background layers from assigned sprites. Use this instead of the removed legacy `CameraFollow.cs`.

## Gameplay

### `Gameplay/IslandMapBuilder.cs`
Builds all selectable island maps: Beginner Island, Foggy Forest, and Volcano Cave. It controls ground tile placement, separated decoration placement, player spawn, generated treasure chests, health pickup generation, and colored key generation.

Important details:
- Ground tiles are placed on the collision tilemap.
- Decorations are placed separately on the decoration tilemap.
- Health pickups, key pickups, and treasure chests are spawned as child GameObjects under generated parent containers.
- Beginner Island generates one yellow key and one yellow chest.
- Foggy Forest generates yellow, red, and green keys/chests; each key opens only the matching chest color.
- Item placement resolves to open platform surfaces so objects do not spawn inside terrain or between overlapping platforms.

### `Gameplay/IslandLevelController.cs`
Tracks whether the current island level has been completed. It listens to all active `TreasureCollectible` objects, exposes `LevelCompleted`, and completes the level only after every generated chest has been opened.

### `Gameplay/TreasureCollectible.cs`
Controls a treasure chest. A chest can require a configured key color before collection. When unlocked, it plays a short star burst effect, then fires the collected event and hides itself.

### `Gameplay/KeyPickup.cs`
Trigger pickup for colored keys. When the player touches it, it adds the configured key color to `PlayerInventory` and disables the pickup object.

### `Gameplay/HealthPickup.cs`
Trigger pickup for health. When the player touches it, it heals `PlayerHealth` and disables the pickup object. Health pickups are not stored in the inventory.

### `Gameplay/PlayerHealth.cs`
Stores current and maximum lives. Supports damage, healing, reset, depletion checks, and a `HealthChanged` event for the HUD.

### `Gameplay/PlayerInventory.cs`
Stores items collected during the current run. Tracks yellow, red, green, and blue key counts. Treasure collection consumes one matching key.

## Player

### `Player/PlayerMovement.cs`
Controls horizontal movement, jumping, crouching, slow crouch movement, facing direction, and sprite-based animation state. It also supports runtime skin changes from the skin selection UI.

## Scent

### `Scent/ScentIndicatorController.cs`
Controls the UI scent arrow. It points toward the nearest uncollected treasure chest when the player is within scent range, grows/darkens as the player gets closer, and hides when all nearby chests are collected or out of range.

## UI

### `UI/HUDManager.cs`
Main game UI and flow controller. It handles:
- Start screen.
- Map selection.
- Skin selection.
- Settings and Help panels.
- Lives display.
- Inventory panel.
- Locked chest message.
- Fall death and respawn/life loss.
- Victory and failure result screens.

## Editor

### `Editor/IslandMapBuilderEditor.cs`
Custom Inspector buttons for `IslandMapBuilder`. Provides one-click controls to build the selected map, build/clear ground, build/clear decorations, and place gameplay objects.

## Removed Legacy Content

The following older prototype scripts/prefabs were removed because the current scene uses the newer systems listed above:

- Legacy `CameraFollow.cs`
- Legacy score `GameManager.cs`
- Legacy `Treasure.cs`
- Legacy `ScentDetector.cs`
- Unused `SentIndicatorStyle.uss`
- Old prototype `Treasure.prefab`
- Old prototype `Square.prefab`
