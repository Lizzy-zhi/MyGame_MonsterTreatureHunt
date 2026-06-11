# Monster Treasure Hunt - Demo and Presentation Notes

## Recommended Demo Length

3 to 5 minutes

## Demo Goal

Show that the project is a coherent vertical slice with:

- a clear start flow
- readable gameplay rules
- multiple maps
- map-specific onboarding prompts
- functioning progression systems
- evidence of design and technical decision-making

## Suggested Demo Structure

### 1. Open with the concept

Suggested explanation:

"Monster Treasure Hunt is a 2D platformer vertical slice where the player controls a clumsy little monster, explores themed maps, collects keys, and unlocks matching treasure chests."

### 2. Show the menu flow

Demonstrate:

- title screen
- map selection
- skin selection
- settings and help

Talking point:

- the game uses a complete front-end flow rather than starting directly inside gameplay

### 3. Show Beginner Island first

Demonstrate:

- movement
- jump
- crouch or crouch-walk
- key collection
- chest unlocking
- the beginner briefing prompt before play starts

Talking point:

- this level acts as a controlled tutorial and introduces the main rule set

### 4. Show Foggy Forest second

Demonstrate:

- multiple colored keys
- inventory panel
- broader routing through a themed level
- the forest briefing prompt before play starts

Talking point:

- this level expands the same core mechanic without changing the overall game logic

### 5. Show Volcano Cave last

Demonstrate:

- hardest traversal
- hazard pressure
- more demanding routing
- the volcano warning prompt before play starts

Talking point:

- this level gives the vertical slice a stronger sense of escalation

## Technical Talking Points

If asked about programming decisions, focus on:

- reusable map builder logic
- shared player, health, inventory, and chest systems
- centralized HUD and panel flow
- level-specific onboarding prompts
- map-specific parallax presentation

## Reflection Talking Points

Useful honest points to mention:

- some early map layouts were too difficult or unreadable and had to be adjusted
- the project improved when scope was reduced
- the original scent mechanic was removed because it was not as strong as the finished platforming loop
- manual playtesting drove many level and UI changes
- the beginner, forest, and volcano prompts were added to improve first-time readability

## Likely Questions and Good Answers

### Why did you keep the scope relatively small?

Because the assignment rewards a polished vertical slice more than a very large unfinished project.

### What changed most during development?

Level layout, pickup placement, and UI clarity changed the most after playtesting.

### What would you improve next?

- add audio
- improve accessibility options
- expand level variety
- deepen the progression system
