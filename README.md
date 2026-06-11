# Monster Treasure Hunt

Monster Treasure Hunt is a 2D Unity platformer about a small monster exploring themed islands, collecting colored keys, opening matching treasure chests, and surviving tricky jumps.

## Documentation Index

This repository now includes a coursework-friendly documentation set:

- [Game Design Document](docs/GAME_DESIGN_DOCUMENT.md)
- [Development Report](docs/DEVELOPMENT_REPORT.md)
- [Testing and QA Notes](docs/TESTING.md)
- [Credits and Compliance Notes](docs/CREDITS_AND_COMPLIANCE.md)
- [Demo and Presentation Notes](docs/DEMO_NOTES.md)
- [Project Structure Guide](docs/PROJECT_STRUCTURE.md)

These files are intended to make the project easier to present, assess, and submit as a complete vertical slice rather than only a source-code repository.

## Current Playable Flow

1. Open `Assets/_Project/Scenes/Main.unity`
2. Press Play in the Unity Editor
3. Click `Start Game`
4. Choose a map: `Beginner Island`, `Foggy Forest`, or `Volcano Cave`
5. Choose a monster skin
6. Read the map-specific briefing prompt and press `Continue`
7. Collect the required keys and unlock every chest in the selected level

## Current Gameplay Features

- English-only UI flow
- Start screen, map selection, skin selection, settings, help, inventory, failure, and victory panels
- Five selectable monster skin colors with preview before gameplay
- Clumsy platforming movement with inertia, skid, jump, crouch, and crouch-walk
- Three-life system with respawn at the last safe ground position
- Color-matched key and chest progression
- Health pickups that restore lost lives
- Chest unlock effect before collection completes
- Level-specific briefing prompts for Beginner Island, Foggy Forest, and Volcano Cave
- Per-map parallax backgrounds built from Kenney art
- Separated map builder controls for ground, decorations, spawn, pickups, hazards, and treasure chests

## Controls

- Move: `A / D` or `Left / Right Arrow`
- Jump: `Space`
- Crouch / crouch-walk: `S` or `Down Arrow`
- Open inventory: `I`
- Open settings: click `Settings` in the top-left corner during gameplay

## Settings Panel

- `Help`: shows the current game rules
- `Continue`: closes the panel and resumes play
- `Escape`: quits Play Mode in the Editor or closes the built game

## Maps in the Current Build

### Beginner Island

- Grass-themed tutorial map
- One yellow key and one yellow chest
- Simple introduction to jumping and chest unlocking
- Two health pickups

### Foggy Forest

- Short forest route with layered jump platforms
- Full-width background river band under the map
- Three chests and three matching keys: yellow, red, and green
- Two health pickups
- Forest parallax background

### Volcano Cave

- Hardest current map
- Full-width background lava band under the map
- Three chests and three matching keys: yellow, red, and green
- Spike hazards
- Three health pickups
- More vertical stepping-platform sections than the earlier maps

## Failure and Victory

- Falling too far below the safe route costs one life
- Spikes also cost one life
- After taking damage, the player respawns at the last safe position
- Reaching zero lives shows the failure state
- Unlocking every chest in the selected map shows the victory screen

## Project Setup

- Unity version: `2022.3.62f3c1`
- Main scene: `Assets/_Project/Scenes/Main.unity`
- Current build settings scene list: `Assets/_Project/Scenes/Main.unity`

## Editor Workflow

To rebuild the active map in the Unity Editor:

1. Open `Assets/_Project/Scenes/Main.unity`
2. Select `Map_Islands/Environment`
3. In the `IslandMapBuilder` inspector, choose the target map theme
4. Click `Build Selected Map`

The custom inspector also includes separate controls for:

- `Build Ground Only`
- `Clear Ground Only`
- `Build Decorations Only`
- `Clear Decorations Only`
- `Place Player Spawn Only`
- `Place Treasure Only`

## Key Scripts

- `Assets/_Project/Scripts/UI/HUDManager.cs`  
  Handles the start flow, map selection, skin selection, level briefings, settings, help text, inventory UI, lives UI, failure state, and victory state.

- `Assets/_Project/Scripts/Levels/IslandMapBuilder.cs`  
  Builds level layouts, tiles, pickups, colored keys, colored chests, hazards, and full-width background water or lava strips.

- `Assets/_Project/Scripts/Levels/IslandLevelController.cs`  
  Tracks chest completion and raises the level-complete state.

- `Assets/_Project/Scripts/Player/Core/PlayerMovement.cs`  
  Handles movement, jump physics, crouch logic, crouch movement, and sprite-based animation changes.

- `Assets/_Project/Scripts/Player/Systems/PlayerHealth.cs`  
  Tracks lives, damage, healing, and damage source state.

- `Assets/_Project/Scripts/Player/Systems/PlayerInventory.cs`  
  Tracks collected keys for the current run.

- `Assets/_Project/Scripts/Gameplay/Collectibles/HealthPickup.cs`  
  Restores one lost life when collected.

- `Assets/_Project/Scripts/Gameplay/Collectibles/KeyPickup.cs`  
  Grants a colored key to the current run.

- `Assets/_Project/Scripts/Gameplay/Collectibles/TreasureCollectible.cs`  
  Handles chest locking, key checks, unlock effects, and collection completion.

- `Assets/_Project/Scripts/Gameplay/Hazards/SpikeHazard.cs`  
  Damages the player when touched.

- `Assets/_Project/Scripts/Editor/Levels/IslandMapBuilderEditor.cs`  
  Provides the custom inspector buttons for rebuilding the selected map and its sub-parts.

- `Assets/_Project/Scripts/Gameplay/Collectibles/TreasureKeyColor.cs`  
  Defines the shared color enum used by keys and chests.

- `Assets/_Project/Scripts/Camera/CameraFollow2D.cs`  
  Handles camera follow behavior and map-specific parallax backgrounds.

## Script Layout

The scripts are now grouped by responsibility:

- `Camera` for camera follow and presentation
- `Editor/Levels` for custom inspector tooling
- `Gameplay/Collectibles` for keys, chests, and pickups
- `Gameplay/Hazards` for damage sources such as spikes
- `Levels` for map generation and level completion flow
- `Player/Core` for movement
- `Player/Systems` for lives and inventory
- `UI` for runtime panels, onboarding prompts, and menu flow

## Art and Assets

This project uses Kenney 2D assets for terrain, props, character sprites, pickups, and background elements.

Primary asset packs included in the repo:

- `Assets/_Project/Art/kenney_new-platformer-pack-1.1`  
  Official source: [Kenney - New Platformer Pack](https://kenney.nl/assets/new-platformer-pack)
- `Assets/_Project/Art/kenney_background-elements-remastered`

## Current Notes

- The original scent-guidance concept is not part of the current playable build.
- The inventory currently tracks keys only.
- Blue key and chest support exists in code, but the current maps only use yellow, red, and green content.
- Beginner Island, Foggy Forest, and Volcano Cave now each open with a short English briefing before play begins.

## Submission-Focused Summary

For coursework purposes, this project is best presented as:

- a complete Unity vertical slice
- three themed levels with increasing challenge
- reusable gameplay systems for movement, health, keys, chests, hazards, and UI
- a project with documented iteration, testing, and scope control

The documentation in the `docs/` folder explains the design choices, technical decisions, testing process, limitations, and presentation plan in a format that is easier to hand in or discuss during assessment.
