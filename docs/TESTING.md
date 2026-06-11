# Monster Treasure Hunt - Testing and QA Notes

## Testing Approach

This project was tested primarily through manual playtesting in the Unity Editor. Because the game is a small real-time platformer, hands-on traversal testing was the most useful method for catching problems with jump distance, object placement, UI timing, and failure states.

Main testing scene:

- `Assets/_Project/Scenes/Main.unity`

Main testing method:

- open the scene in Unity
- rebuild the selected map when needed
- press Play
- complete the full level loop from start menu to victory or failure

## Core Test Checklist

| Area | Test | Expected Result | Status |
| --- | --- | --- | --- |
| Start flow | Click `Start Game` | Map selection opens correctly | Covered |
| Map selection | Choose each of the three maps | Correct level loads for Beginner Island, Foggy Forest, and Volcano Cave | Covered |
| Level briefing | Confirm each map prompt | Beginner Island, Foggy Forest, and Volcano Cave each show the correct pre-start briefing | Covered |
| Prompt continue | Press `Continue` on the briefing | Gameplay starts only after the prompt is dismissed | Covered |
| Skin selection | Choose each monster skin | Preview updates and selected skin appears in gameplay | Covered |
| Movement | Move left and right | Character moves with intended inertia and stop behavior | Covered |
| Jumping | Traverse main routes | Required jumps are possible without impossible precision | Covered after iteration |
| Crouch | Enter low spaces | Character can crouch and crouch-walk through intended passages | Covered |
| Fall damage | Drop below safe route | Player loses one life and respawns at the last safe position | Covered |
| Hazard damage | Touch spikes where present | Player loses one life and respawns unless lives are depleted | Covered |
| Health pickups | Collect a heart pickup after taking damage | Lost life is restored and pickup disappears | Covered |
| Key pickups | Collect colored keys | Inventory count updates correctly | Covered |
| Chest unlocking | Touch a chest with the matching key | Chest unlock effect plays and progress updates | Covered |
| Wrong chest order | Reach a locked chest without the right key | Locked feedback appears and chest remains closed | Covered |
| Inventory UI | Press `I` during gameplay | Inventory panel opens and shows collected keys clearly | Covered |
| Settings UI | Open settings from the top-left button | Help, Continue, and Escape actions appear correctly | Covered |
| Failure state | Lose all lives | Failure panel appears with game-over messaging | Covered |
| Victory state | Unlock all required chests in a map | Victory panel appears and the level is considered complete | Covered |

## Main Issues Found During Development

### Traversal problems

Problem:

- some platforms were too high or too far apart for the real jump arc
- some routes looked possible but were not actually passable

Fixes:

- reduced extreme gaps
- adjusted stepped platform layouts
- tuned some second-level routes so the game stayed challenging without becoming unfair

### Pickup placement problems

Problem:

- some hearts or keys sat too low or too close to nearby objects
- some pickups did not look easy to collect

Fixes:

- moved pickups to clearer surfaces
- adjusted scale and placement offsets

### Background strip alignment problems

Problem:

- river or lava elements could feel visually inconsistent when not aligned as a clean band

Fixes:

- standardized them as wider full-width strips at the bottom of the map presentation

### UI clarity problems

Problem:

- some menu wording and inventory layout were harder to read than necessary
- some input behavior caused settings-related confusion

Fixes:

- rewrote help text
- cleaned up runtime UI arrangement
- refined settings interaction behavior
- added short map-specific briefing prompts before the three levels

## Regression Checks Worth Running Before Submission

Before the final hand-in, the following checks should be repeated once more:

1. Play all three maps from the title screen and confirm they are completable.
2. Verify that every required key is reachable and every matching chest can be opened.
3. Confirm that lives decrease correctly on fall death and spike damage.
4. Confirm that health pickups are visible and collectable.
5. Confirm that the inventory panel shows the correct key counts for the current map.
6. Confirm that victory and failure screens still appear after recent content changes.
7. Confirm that parallax backgrounds and lower water/lava strips do not overlap the playable ground in a distracting way.
8. Confirm that each map still shows the correct opening briefing prompt.

## Testing Limitations

- testing is manual rather than automated
- results depend on current map data and scene wiring
- no formal performance profiling has been documented yet

For a vertical slice coursework project, this manual QA approach is still useful, but the limitation should be acknowledged honestly in the report or presentation.
