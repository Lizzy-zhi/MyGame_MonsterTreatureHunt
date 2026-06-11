# Monster Treasure Hunt - Game Design Document

## Project Summary

Monster Treasure Hunt is a small 2D Unity platformer vertical slice. The player controls a clumsy little monster who explores themed islands, gathers keys, opens matching treasure chests, and survives platforming hazards.

The project is intentionally scoped as a short, polished coursework game rather than a large open-ended production. The goal is to demonstrate a complete gameplay loop, clear user feedback, readable level progression, and steady technical improvement.

## Design Goal

The intended player experience is:

- easy to understand in the first minute
- playful and slightly challenging
- visually varied across three themed maps
- complete enough to feel like a real game slice rather than isolated prototypes

The game focuses on short-form exploration and collection. Each map asks the player to read the terrain, make manageable jumps, find keys, and unlock all required treasure chests.

## Core Pillars

1. Readable platforming  
   Movement should feel responsive enough to control, but still a little clumsy to support the monster theme.

2. Clear progression  
   The player always has a simple goal: collect keys, open matching chests, and reach completion.

3. Strong feedback  
   UI, pickup visuals, chest states, failure panels, and victory panels should clearly tell the player what is happening.

4. Small but complete scope  
   Three themed maps, one player character system, a compact UI flow, and a few reusable gameplay systems are enough for a convincing vertical slice.

## Target Platform and Audience

- Platform: Windows PC / Unity Editor play mode
- Control scheme: keyboard
- Audience: players who enjoy simple platforming, collectible goals, and short challenge maps

## Core Gameplay Loop

1. Start the game
2. Choose a map
3. Choose a monster skin
4. Explore the map and survive jumps or hazards
5. Collect colored keys
6. Use the correct key to unlock matching chests
7. Recover from mistakes through lives and respawns
8. Unlock every chest to clear the level

## Implemented Mechanics

### Player Movement

- left/right movement
- jump
- crouch
- crouch-walk
- inertia and skid to support the "clumsy monster" feel

### Health and Failure

- three lives per run
- damage from falling too far
- damage from spike hazards where used
- respawn at the last safe ground position after losing a life
- failure screen when lives reach zero

### Keys and Treasure Chests

- color-matched key and chest system
- the player must collect the correct key before a chest can be opened
- unlocking all required chests completes the level

### Pickups

- key pickups
- health pickups that restore a lost life

### UI Flow

- title screen
- map selection
- skin selection with preview
- level-specific briefing prompts
- settings panel
- help panel
- inventory panel
- failure panel
- victory panel

### Level Themes

- Beginner Island: tutorial-style grass map
- Foggy Forest: forest route with multiple keys and chests
- Volcano Cave: hardest map with hazards and more demanding platforming

## Level Design Intent

### Beginner Island

Purpose:

- teach the controls
- introduce one-key, one-chest progression
- keep the route readable and forgiving
- open with a short beginner guide before gameplay starts

### Foggy Forest

Purpose:

- introduce multiple colored objectives
- increase route variety
- encourage the player to check inventory and plan a path
- open with a briefing prompt that explains the multi-key route

### Volcano Cave

Purpose:

- provide the highest challenge in the current build
- combine key routing, hazards, and more layered jumps
- warn the player about spikes and knockback before play begins

## Scope Decisions

Several design choices were made to keep the project achievable and polished:

- the game is single-player only
- the build uses three compact maps instead of a large world
- the original scent-guidance idea was cut from the final playable version
- the inventory only tracks keys because that is the mechanic the player actually needs
- progression is built around map completion rather than a long upgrade system

These cuts improved coherence and reduced the risk of unfinished systems.

## Tools and Resources

- Engine: Unity `2022.3.62f3c1`
- Programming language: C#
- Version control: Git and GitHub
- Art sources: Kenney asset packs already included in the project

## Technical Strategy

The project uses reusable gameplay systems instead of building each map by hand:

- a map builder script assembles map content
- reusable key, chest, pickup, and hazard components support all levels
- a shared HUD controller manages menus, runtime panels, and map briefings
- parallax background support allows each map to feel visually distinct

This approach makes the vertical slice easier to extend while keeping the code organized.

## Legal, Ethical, Accessibility, and Security Considerations

- third-party art is credited and sourced from Kenney packs included in the repository
- the game does not collect personal data or use online networking
- the controls are intentionally simple and introduced through help text
- UI labels support the color-coded key system, which is helpful for clarity

Current limitations remain:

- no remappable controls
- no dedicated accessibility menu
- no audio cues for players who benefit from multimodal feedback

## Success Criteria

The project is successful if it demonstrates:

- a playable and stable Unity build
- a complete start-to-finish gameplay loop
- multiple themed maps with increasing challenge
- clear rule communication
- visible evidence of iteration and improvement
