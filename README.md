# Monster Treasure Hunt

Monster Treasure Hunt is a 2D Unity platformer about a small monster exploring themed islands, collecting colored keys, opening matching treasure chests, and surviving tricky jumps.

## Current Playable Flow

1. Open `Assets/Main.unity`
2. Press Play in the Unity Editor
3. Click `Start Game`
4. Choose a map: `Beginner Island`, `Foggy Forest`, or `Volcano Cave`
5. Choose a monster skin
6. Collect the required keys and unlock every chest in the selected level

## Current Gameplay Features

- English-only UI flow
- Start screen, map selection, skin selection, settings, help, inventory, failure, and victory panels
- Five selectable monster skin colors with preview before gameplay
- Clumsy platforming movement with inertia, skid, jump, crouch, and crouch-walk
- Three-life system with respawn at the last safe ground position
- Color-matched key and chest progression
- Health pickups that restore lost lives
- Chest unlock effect before collection completes
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
- Main scene: `Assets/Main.unity`
- Current build settings scene list: `Assets/Main.unity`

## Editor Workflow

To rebuild the active map in the Unity Editor:

1. Open `Assets/Main.unity`
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
  Handles the start flow, map selection, skin selection, settings, help text, inventory UI, lives UI, failure state, and victory state.

- `Assets/_Project/Scripts/Gameplay/IslandMapBuilder.cs`  
  Builds level layouts, tiles, pickups, colored keys, colored chests, hazards, and full-width background water or lava strips.

- `Assets/_Project/Scripts/Player/PlayerMovement.cs`  
  Handles movement, jump physics, crouch logic, crouch movement, and sprite-based animation changes.

- `Assets/_Project/Scripts/Gameplay/PlayerHealth.cs`  
  Tracks lives, damage, healing, and damage source state.

- `Assets/_Project/Scripts/Gameplay/PlayerInventory.cs`  
  Tracks collected keys for the current run.

- `Assets/_Project/Scripts/Gameplay/TreasureCollectible.cs`  
  Handles chest locking, key checks, unlock effects, and collection completion.

- `Assets/_Project/Scripts/Camera/CameraFollow2D.cs`  
  Handles camera follow behavior and map-specific parallax backgrounds.

## Art and Assets

This project uses Kenney 2D assets for terrain, props, character sprites, pickups, and background elements.

## Current Notes

- The original scent-guidance concept is not part of the current playable build.
- The inventory currently tracks keys only.
- Blue key and chest support exists in code, but the current maps only use yellow, red, and green content.
