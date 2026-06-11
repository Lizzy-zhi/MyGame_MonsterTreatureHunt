# Monster Treasure Hunt - Game Design Document

## Summary

Monster Treasure Hunt is a short 2D Unity platformer vertical slice. The player controls a clumsy little monster, explores themed maps, collects colored keys, opens matching chests, and survives hazards.

## Design Goals

- Make the first minute easy to understand.
- Use three visually different maps with rising difficulty.
- Keep goals clear: find keys, open chests, survive, and finish.
- Present a complete small game rather than a large unfinished prototype.

## Core Loop

1. Choose a map and skin.
2. Read the level briefing.
3. Traverse platforms and avoid hazards.
4. Collect keys and health pickups.
5. Open matching treasure chests.
6. Clear the level or lose all lives.

## Mechanics

- Movement: left/right, jump, crouch, crouch-walk, ladder climb.
- Health: three lives, damage, healing, respawn, failure screen.
- Progression: colored keys unlock matching chests.
- UI: start flow, map select, skin select, briefings, help, inventory, victory, and failure.
- Hazards/enemies: spikes, bee, jumping fish, fire slimes, fake chest explosion.

## Level Roles

### Beginner Island

Tutorial map with simple jumps, one yellow key, one yellow chest, and forgiving pacing.

### Foggy Forest

Middle map with three colored keys/chests, layered platforms, one early jumping fish, and one bee that requires crouching.

### Volcano Cave

Hardest map with spikes, fire slimes, ladders, a fake green chest trap, lava presentation, and more vertical routes.

## Scope Decisions

- Single-player only.
- Three compact maps instead of a large world.
- Inventory tracks keys only.
- The original scent-guidance idea was removed to keep the final build focused.

## Success Criteria

- The game can be played from menu to victory/failure.
- Each map has a clear theme and challenge level.
- Keys, chests, hazards, lives, and UI feedback work consistently.
- The project shows iteration and a controlled final scope.
