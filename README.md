# Monster Treasure Hunt

Monster Treasure Hunt is a 2D Unity platformer where a small monster explores three themed maps, collects colored keys, opens matching treasure chests, and survives hazards.

## Docs

- [Game Design Document](docs/GAME_DESIGN_DOCUMENT.md)
- [Development Report](docs/DEVELOPMENT_REPORT.md)
- [Testing Notes](docs/TESTING.md)
- [Credits and Compliance](docs/CREDITS_AND_COMPLIANCE.md)
- [Project Structure](docs/PROJECT_STRUCTURE.md)

## Play Flow

1. Open `Assets/_Project/Scenes/Main.unity`.
2. Press Play.
3. Click `Start Game`.
4. Choose `Beginner Island`, `Foggy Forest`, or `Volcano Cave`.
5. Choose a monster skin.
6. Read the level briefing and press `Continue`.
7. Collect keys, open matching chests, and clear the level.

## Controls

- Move: `A / D` or `Left / Right Arrow`
- Jump: `Space`
- Crouch / crouch-walk: `S` or `Down Arrow`
- Climb ladders: `Up / Down Arrow`
- Inventory: `I`
- Settings: top-left `Settings` button during gameplay

## Current Features

- English UI with start, map select, skin select, settings, help, inventory, failure, and victory screens.
- Five monster skins with preview.
- Clumsy movement with jump, crouch, crouch-walk, and ladder climbing.
- Three lives, health pickups, respawn after damage, and source-specific failure messages.
- Colored keys and matching treasure chests.
- Kenney-based tiles, characters, enemies, pickups, backgrounds, and UI assets.

## Maps

### Beginner Island

- Tutorial grass map.
- One yellow key and one yellow chest.
- Simple jumps, crouch movement, and two health pickups.

### Foggy Forest

- Layered forest route with a full-width river background.
- Yellow, red, and green keys/chests.
- One jumping fish in the first water gap before the yellow key.
- One bee enemy that teaches crouch-dodging.
- Two health pickups.

### Volcano Cave

- Hardest map with stone terrain and full-width lava background.
- Yellow, red, and green keys/chests.
- Spikes, fire slimes, ladders, and a fake green chest that explodes.
- Three health pickups.

## Editor Workflow

To rebuild a map:

1. Open `Assets/_Project/Scenes/Main.unity`.
2. Select `Map_Islands/Environment`.
3. Choose the map theme in `IslandMapBuilder`.
4. Click `Build Selected Map`.

The custom inspector can also rebuild ground, decorations, spawn, treasure, bee enemies, fish enemies, fire slime enemies, and ladders separately.

## Key Scripts

- `HUDManager.cs`: UI flow, briefings, inventory, lives, hints, victory, and failure.
- `IslandMapBuilder.cs`: map tiles, pickups, keys, chests, hazards, enemies, ladders, water, and lava.
- `PlayerMovement.cs`: movement, jump, crouch, ladders, hurt feedback, and animation sprites.
- `PlayerHealth.cs`: lives, healing, damage, and damage source.
- `PlayerInventory.cs`: collected keys.
- `TreasureCollectible.cs`: chest locks, key checks, unlock effects, and completion.
- `BeeEnemy.cs`, `FishEnemy.cs`, `FireSlimeEnemy.cs`, `SpikeHazard.cs`, `FakeTreasureChestTrap.cs`: hazards and enemies.
- `CameraFollow2D.cs`: camera follow and parallax backgrounds.

## Notes

- Unity version: `2022.3.62f3c1`
- Main scene: `Assets/_Project/Scenes/Main.unity`
- The original scent-guidance idea is not part of the current playable build.
- Audio is not currently implemented.
