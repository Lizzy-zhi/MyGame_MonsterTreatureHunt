# Monster Treasure Hunt - Testing Notes

## Approach

Testing was manual in the Unity Editor using `Assets/_Project/Scenes/Main.unity`. The main focus was whether the game could be played from the menu to victory/failure and whether map routes were actually passable.

## Core Checklist

| Area | Expected Result | Status |
| --- | --- | --- |
| Start flow | Start, map select, skin select, and briefing screens work | Covered |
| Controls | Move, jump, crouch, crouch-walk, and ladders work | Covered |
| Beginner Island | Yellow key and yellow chest route is completable | Covered |
| Foggy Forest | Three keys/chests are reachable and completable | Covered |
| Foggy fish | Only appears in the first water gap before the yellow key | Covered |
| Foggy bee | Damages player and shows crouch hint before contact | Covered |
| Volcano Cave | Keys, chests, ladders, slimes, spikes, and fake chest are playable | Covered |
| Health | Damage removes lives; hearts restore lives | Covered |
| Inventory | `I` shows collected keys clearly | Covered |
| Locked chests | Missing-key message appears | Covered |
| Victory | Unlocking all real chests completes the map | Covered |
| Failure | Losing all lives shows a source-specific failure message | Covered |

## Issues Fixed

- Platforms that were too high or too far apart were adjusted.
- Hearts and keys were moved when they overlapped terrain or decoration.
- Water and lava were standardized as full-width lower strips.
- Bee hint timing was moved earlier.
- Fish was limited to one early water gap and made visible in editor preview.
- Ladder top-exit logic was added so the player can climb onto high platforms.
- Volcano spike and platform spacing were adjusted after playtesting.

## Final Regression Checks

Before submission, replay:

1. Beginner Island from start to victory.
2. Foggy Forest from start to victory, checking the early fish and bee.
3. Volcano Cave from start to victory, checking ladders, slimes, spikes, and fake chest.
4. Failure state by losing all lives.
5. Inventory, settings, help, and victory panels.

## Limitations

- Testing is manual, not automated.
- No formal performance profiling is documented.
- No automated route validation exists for every jump.
