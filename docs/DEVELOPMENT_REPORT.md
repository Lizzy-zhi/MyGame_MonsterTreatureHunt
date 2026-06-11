# Monster Treasure Hunt - Development Report

## Overview

Monster Treasure Hunt began as a simple treasure-hunting platformer concept built around an adorable monster character and short, themed levels. During development, the project moved away from a broader idea and toward a more focused vertical slice with three maps, a clear UI flow, and a compact set of mechanics that work together consistently.

The final result is a small but complete single-player platformer that demonstrates movement, level progression, pickups, hazards, feedback screens, and map-specific presentation.

The main playable scene now lives under `Assets/_Project/Scenes/Main.unity`, and the scripts were reorganized into clearer responsibility-based folders during the final cleanup pass.

## How the Design Evolved

The earliest concept aimed for a treasure-hunting fantasy with a smell-guidance mechanic. As development progressed, it became clearer that the strongest part of the project was not the scent system but the combination of:

- clumsy monster movement
- collectible keys
- matching treasure chests
- short themed maps with increasing challenge

The scent-guidance idea was therefore removed from the final build. This was an important scope decision. Keeping an unfinished or weakly integrated feature would have made the game feel less coherent, while cutting it helped the project become a more polished platformer vertical slice.

## Key Design Choices

### 1. Three-map structure

Instead of making one long level or several incomplete scenes, the game uses three themed maps:

- Beginner Island
- Foggy Forest
- Volcano Cave

This structure supports a clear progression from tutorial to challenge while keeping the amount of content realistic for the module.

### 2. Color-matched progression

The key-and-chest system gives each level a simple, readable objective. It also creates a stronger gameplay loop than collecting a single generic treasure because the player has to route through the level and understand what is still locked.

### 3. Clumsy movement style

The player does not move like a perfectly sharp arcade character. Inertia, skid, crouch, and crouch-walk were kept because they fit the monster theme and make traversal feel more distinctive.

### 4. Strong UI support

The project uses clear panels for:

- map selection
- skin selection
- level-specific briefing prompts
- settings
- help
- inventory
- failure
- victory

This was important because the assignment rewards coherent player experience, not just raw mechanics.

## Key Technical Decisions

### Reusable map builder

One of the most important technical decisions was using a map builder system rather than scattering unique scene-only logic across the project. This supports:

- consistent tile placement
- separated control of ground and decoration
- repeatable placement of pickups, keys, chests, and hazards
- easier iteration when map jumps or object positions need adjustment

### Shared gameplay components

The game uses reusable scripts for health, inventory, pickups, hazards, and chest logic. This is a better choice than embedding all rules into a single large script because it keeps responsibilities clearer and makes level content easier to extend.

### Shared HUD controller

A single HUD manager handles most menu and runtime interface behavior. This centralizes UI flow and helps keep the game state consistent during start, pause, level briefing, failure, and victory situations.

### Folder-based script organization

The scripts were later grouped into `Levels`, `Player/Core`, `Player/Systems`, `Gameplay/Collectibles`, `Gameplay/Hazards`, `Editor/Levels`, `Camera`, and `UI` so the repository reads more clearly as a finished project.

## Problems Encountered

Several issues appeared during development and required iteration:

### 1. Platform layouts were sometimes not traversable

Some earlier map versions included jumps that were too high, too wide, or awkward for the actual player size and jump arc. This made levels frustrating or impossible to complete.

Response:

- platform spacing was repeatedly adjusted
- important routes were simplified where needed
- jump sections were changed to stay within a more consistent difficulty range

### 2. Pickups could become visually or physically awkward

Some health pickups or keys were placed too close to ground or decoration, which reduced readability and sometimes made them feel embedded in the terrain.

Response:

- pickup positions were moved to cleaner surfaces
- object scale and offset were tuned for visibility

### 3. Decoration and terrain could drift out of alignment

Because the project uses Kenney tiles and layered map content, some earlier versions had decorative elements that did not sit naturally on the ground or that visually clashed with gameplay tiles.

Response:

- decoration and ground control were separated
- some maps were simplified
- background water and lava strips were standardized to create cleaner silhouettes

### 4. UI behavior needed cleanup

Certain interactions, such as settings behavior, help wording, and inventory layout, needed refinement to make the experience clearer and less confusing.

Response:

- settings actions were reorganized
- help text was rewritten in clearer English
- inventory layout was improved so text and icons read more cleanly
- the opening flow was extended with short map-specific briefings for the three levels

## Evidence of Testing and Iteration

The project was improved through repeated manual testing in the Unity Editor. Testing focused on:

- whether jumps were possible
- whether crouch passages worked
- whether keys and chests could be reached in the correct order
- whether falling correctly reduced lives and triggered respawn
- whether pickups and hazards gave clear feedback
- whether UI panels appeared at the right time

Changes made after testing included:

- shortening or adjusting maps that felt too long or unfair
- moving unreachable pickups
- reworking some second-layer platform spacing
- replacing inconsistent background strip placement with cleaner full-width bands
- improving the failure and inventory feedback loop
- adding beginner, forest, and volcano briefing prompts before play begins

## Current Strengths

- clear gameplay loop
- multiple maps with distinct themes
- consistent collectible progression
- better presentation than a prototype-only submission
- visible evidence of scope control and iteration
- clearer onboarding through level-specific prompts

## Current Limitations

- the game does not include sound
- controls are not remappable
- the final build does not use the original scent-guidance concept
- testing is manual rather than automated
- the project is a vertical slice, so content depth is intentionally limited

## Reflection

The most important development lesson from this project was that reducing scope improved quality. Removing weaker ideas and focusing on movement, keys, chests, and readable level flow made the game more complete and more presentable.

From a coursework perspective, the project is strongest when presented as a polished vertical slice with clear iteration, rather than as a much larger but unfinished design ambition.
