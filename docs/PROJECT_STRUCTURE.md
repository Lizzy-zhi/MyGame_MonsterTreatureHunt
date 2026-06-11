# Monster Treasure Hunt - Project Structure

## Main Paths

- `Assets/_Project/Scenes/Main.unity`: main playable scene.
- `Assets/_Project/Scripts`: custom game scripts.
- `Assets/_Project/Art`: imported Kenney assets.
- `Assets/_Project/UI`: UI Toolkit layout and styles.
- `docs`: short project documentation.

## Script Folders

- `Camera`: camera follow and parallax presentation.
- `Editor/Levels`: custom map builder inspector.
- `Gameplay/Collectibles`: keys, health pickups, treasure chests, and key colors.
- `Gameplay/Hazards`: spikes, bee, fish, fire slime, fake chest trap, and ladder zones.
- `Levels`: map generation and level completion.
- `Player/Core`: player movement.
- `Player/Systems`: health and inventory.
- `UI`: menu flow, HUD, prompts, inventory, failure, and victory.

## Key Scripts

- `HUDManager.cs`: start flow, map/skin selection, help, inventory, lives, hints, victory, and failure.
- `IslandMapBuilder.cs`: builds maps and places pickups, keys, chests, hazards, enemies, ladders, water, and lava.
- `IslandLevelController.cs`: tracks level completion.
- `PlayerMovement.cs`: movement, crouch, ladders, hurt feedback, and animation changes.
- `PlayerHealth.cs`: lives, damage, healing, and damage source.
- `PlayerInventory.cs`: collected keys.
- `TreasureCollectible.cs`: chest lock checks, unlock effects, and collection.
- `BeeEnemy.cs`: Foggy Forest bee and crouch hint.
- `FishEnemy.cs`: jumping fish in the first Foggy Forest water gap.
- `FireSlimeEnemy.cs`: Volcano Cave slime patrol and damage.
- `FakeTreasureChestTrap.cs`: fake green chest transformation and explosion.
- `LadderZone.cs`: ladder climbing trigger and top exit.
- `CameraFollow2D.cs`: camera follow and background theme.

## Editor Tools

`IslandMapBuilderEditor.cs` adds buttons to rebuild:

- full selected map
- ground
- decorations
- player spawn
- treasure
- bee enemies
- fish enemies
- fire slime enemies
- ladders
