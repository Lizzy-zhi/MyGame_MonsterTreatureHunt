# Monster Treasure Hunt - Project Structure Guide

## Repository Overview

This repository is organized around a single Unity project with one main scene and a focused custom project folder.

## Key Root Folders

### `Assets`

Contains the playable game content, including scenes, scripts, art, tiles, prefabs, and UI assets.

### `Packages`

Unity package dependencies for the project.

### `ProjectSettings`

Unity editor and project configuration.

### `docs`

Coursework-facing documentation, including design, testing, credits, and demo notes.

## Important Game Content Areas

### `Assets/_Project/Scenes/Main.unity`

The main playable scene used for the current build.

### `Assets/_Project/Scripts`

Main custom gameplay code.

Recommended high-level reading order:

- `UI/` for flow and player-facing panels
- `Levels/` for map generation and level completion
- `Gameplay/Collectibles/` and `Gameplay/Hazards/` for interactables and damage sources
- `Player/` for movement and player state
- `Camera/` for follow and parallax behavior
- `Editor/` for inspector helpers and editor tools

### `Assets/_Project/Art`

Custom project art references and imported Kenney asset packs.

## Important Scripts

### `Assets/_Project/Scripts/UI/HUDManager.cs`

Controls the title flow, map selection, skin selection, level briefings, settings, help text, inventory, lives display, failure screen, and victory screen.

### `Assets/_Project/Scripts/Levels/IslandMapBuilder.cs`

Builds map tiles and places gameplay objects such as keys, chests, hazards, pickups, and lower background strips.

### `Assets/_Project/Scripts/Levels/IslandLevelController.cs`

Tracks whether the active level has been cleared.

### `Assets/_Project/Scripts/Player/Core/PlayerMovement.cs`

Handles movement, jump behavior, crouch, crouch-walk, and animation state changes.

### `Assets/_Project/Scripts/Player/Systems/PlayerHealth.cs`

Tracks remaining lives, applies damage, handles healing, and supports respawn logic.

### `Assets/_Project/Scripts/Player/Systems/PlayerInventory.cs`

Tracks currently collected keys and supports chest unlocking rules.

### `Assets/_Project/Scripts/Gameplay/Collectibles/HealthPickup.cs`

Restores one lost life when collected.

### `Assets/_Project/Scripts/Gameplay/Collectibles/KeyPickup.cs`

Grants a colored key to the current run.

### `Assets/_Project/Scripts/Gameplay/Collectibles/TreasureCollectible.cs`

Manages chest requirements, unlock effects, and successful collection state.

### `Assets/_Project/Scripts/Gameplay/Hazards/SpikeHazard.cs`

Damages the player when touched.

### `Assets/_Project/Scripts/Editor/Levels/IslandMapBuilderEditor.cs`

Provides the custom inspector buttons used to rebuild the selected map and its sub-parts inside the Unity Editor.

### `Assets/_Project/Scripts/Gameplay/Collectibles/TreasureKeyColor.cs`

Defines the shared color enum used by keys and chests.

### `Assets/_Project/Scripts/Camera/CameraFollow2D.cs`

Handles camera follow behavior and theme-based parallax presentation.

## Why This Structure Works Well

- gameplay code is grouped by responsibility
- the main scene is easy to identify
- art sources are easy to audit and credit
- map-specific briefing prompts are handled by the HUD instead of being spread across scenes
- coursework documentation is separated from runtime project files

This makes the repository easier for a marker, tutor, or collaborator to navigate quickly.
