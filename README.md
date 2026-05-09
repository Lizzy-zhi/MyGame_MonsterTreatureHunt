# MyGame_MonsterTreatureHunt
“Monster Treasure Hunt” Game Plan and UI Design
**Core Design**
Players take on the role of a clumsy and adorable little monster on a hand-drawn style island, using a smell detection system to find hidden treasures, avoid traps, and ultimately become the island's treasure hunt king.
**Core Mechanics**：
1. Smell-based Treasure Hunt System (core gameplay)
Visuals: “Scent indicator arrows” appear at the screen edges; the closer to the treasure, the bigger and darker the arrows
Game Logic: Treasures emit “scent particles,” triggering guidance when the player enters range
Technical Implementation: Unity Particle System + UI arrow pointing
2. Clumsy Physics System (characteristic feel)
Monster movement has inertia, slow turning, and skidding when stopping suddenly
Adds fun with physical feedback
3. Treasure Collection and Upgrades
Collecting treasures unlocks new appearances (colors, hats)
Simple progression system
**Game Structure** 
Level Design: 3 islands
--**Beginner Island** (tutorial level)
1 treasure, no traps
Teaching: movement, smell system
--**Foggy Forest**
3 treasures, 2 types of traps (mud pits, rolling stones)
New: scents are blown by the wind, increasing difficulty
--**Volcano Cave**
5 treasures, complex traps
Final level, combines all mechanics
