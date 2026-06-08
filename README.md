# Monster Treasure Hunt

Players control a clumsy little monster on a island, using scent guidance to find treasure, explore themed maps, and unlock character skins.

## Current Build Status

### Done

- Start flow: Start Game -> map selection -> skin selection -> gameplay.
- Settings button opens Help, Continue, and Escape controls.
- Help panel shows game rules.
- Character skins can be previewed and confirmed before entering gameplay.
- Player movement includes inertia, skid feel, jumping, crouching, and slow crouch movement.
- Treasure collection triggers level clear text.
- Falling too far below the spawn height shows a failure icon and stops player input.
- Ground, decoration, player spawn, and treasure placement are controlled separately in the map builder.
- Camera background uses Kenney parallax layers for stronger movement feeling.

### Maps

- Beginner Island: playable tutorial map with one treasure and no traps.
- Foggy Forest: compact second map layout built with Kenney tiles, forest decorations, one treasure near the end, and stable grid colliders on ground tiles.
- Volcano Cave: placeholder layout exists, but final theme, traps, and treasure count still need design work.

### In Progress

- Foggy Forest polish pass in Unity Play Mode:
  - Verify every main platform is standable.
  - Confirm the treasure is reachable from the spawn.
  - Check camera framing and parallax background movement across the route.

### Backlog

- Foggy Forest gameplay upgrade:
  - Add 3 treasures.
  - Add mud pits and rolling stones.
  - Add wind influence to scent guidance.
- Volcano Cave:
  - Build final cave layout.
  - Add 5 treasures.
  - Add complex traps.
- Progression:
  - Unlock skins or hats after collecting treasures.
  - Save selected skin and unlocked rewards.
- UI polish:
  - Add retry/restart action on failure.
  - Add clearer level completion panel.
  - Add map-specific preview art on map selection.

## Core Mechanics

1. Smell-based treasure hunt system
   - Scent indicator arrows appear near the screen edge.
   - The arrow guides the player toward the active treasure.

2. Clumsy physics system
   - Movement has inertia and skid when stopping or turning.
   - Crouching changes the collider and allows slow movement.

3. Treasure collection and upgrades
   - Current build supports treasure collection and skin selection.
   - Future work should connect treasure progress to unlockable appearances.

## Level Plan

- Beginner Island: tutorial level, one treasure, no traps.
- Foggy Forest: forest level, planned three treasures and two trap types.
- Volcano Cave: final level, planned five treasures and more complex trap combinations.
