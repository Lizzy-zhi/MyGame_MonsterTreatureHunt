# Monster Treasure Hunt - Development Report

## Overview

Monster Treasure Hunt began as a broader treasure-hunting idea and became a focused 2D platformer vertical slice. The final build has three maps, a complete UI flow, movement, lives, keys, chests, pickups, hazards, enemies, and victory/failure states.

The original scent-guidance mechanic was removed because it was less complete than the platforming, key, and chest systems. Cutting it made the final game clearer and more polished.

## Main Design Decisions

- Use three maps: Beginner Island, Foggy Forest, and Volcano Cave.
- Keep progression simple: collect colored keys and open matching chests.
- Give the player clumsy monster movement with inertia, skid, crouch, crouch-walk, jump, and ladder climbing.
- Add hazards gradually: fish and bee in Foggy Forest, then spikes, fire slimes, ladders, and fake chest trap in Volcano Cave.
- Use short English briefing prompts so players understand each map before moving.

## Technical Decisions

- `IslandMapBuilder` generates tiles, pickups, keys, chests, enemies, hazards, ladders, water, and lava.
- Reusable scripts handle health, inventory, pickups, hazards, enemies, traps, and chest rules.
- `HUDManager` controls the start flow, briefings, settings, help, inventory, lives, hints, failure, and victory.
- Scripts were organized into clear folders by responsibility.

## Problems and Fixes

- Some jumps were impossible or unfair, so platform spacing and heights were adjusted.
- Some pickups overlapped ground or decoration, so positions and scale were tuned.
- Water and lava looked inconsistent, so they were changed into full-width lower strips.
- Ladder tops could trap the player, so ladder top-exit behavior was added.
- The bee needed clearer warning, so a crouch hint was added before the bee platform.
- The fish was hard to see in editor view, so preview visibility was added and it was limited to the first water gap.
- Volcano Cave difficulty was adjusted through spike, platform, ladder, slime, and fake chest placement.

## Current Strengths

- Complete menu-to-level-to-result flow.
- Three maps with clear escalation.
- Readable key/chest progression.
- Level-specific hazards and enemies.
- Organized scripts and concise documentation.

## Current Limitations

- No audio is implemented.
- Controls are not remappable.
- Testing is manual rather than automated.
- The game is a vertical slice, so content depth is limited.

## Reflection

The project improved when the scope became smaller and clearer. Focusing on platforming, keys, chests, hazards, and UI produced a more complete result than trying to keep every early idea.
