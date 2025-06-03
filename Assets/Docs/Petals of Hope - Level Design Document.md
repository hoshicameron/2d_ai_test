# **Petals of Hope \- Level Design Document**

## **1\. Introduction**

This document details the level design philosophy, structure, and specific elements for *Petals of Hope*, expanding upon the Game Design Document (GDD). The goal is to create engaging, progressively challenging levels that guide the player through Elias's journey across four distinct environments.

## **2\. Level Design Philosophy**

* **Player Guidance:** Levels are designed to intuitively guide the player forward while encouraging exploration of optional paths. Visual cues (e.g., coin trails, distinct platform colors) will subtly direct the player.  
* **Pacing:** Each level balances platforming challenges, enemy encounters, and moments of respite or exploration. Difficulty increases gradually within each environment and ramps up significantly between environments.  
* **Skill Introduction:** New mechanics (Double Jump, Wall Grab/Jump, Dash) are introduced after boss fights and subsequent levels are designed to teach and test these new abilities in increasingly complex scenarios.  
* **Environmental Storytelling:** Level layouts, hazards, and enemy placements reflect the theme and narrative of each environment (e.g., dense foliage and water hazards in the Forest, heat effects and ruins in the Desert).  
* **Reward Exploration:** Hidden areas contain valuable collectibles (coins, health items) or optional, tougher challenges, rewarding curious players.

## **3\. Overall Structure**

* **Total Levels:** 40  
* **Environments:** 4 (Forest, Desert, Mountain, Cave), 10 levels each.  
* **Boss Levels:** Levels 10, 20, 30, and 40 are dedicated boss arenas.  
* **Progression:** Linear main path with branching optional/hidden paths.

## **4\. Environment Breakdown**

### **4.1. Forest (Levels 1-10)**

* **Theme:** Lush, vibrant, introductory. Focus on basic platforming and combat.  
* **Mechanics Introduced:** Walking, Jumping, Basic Combat (Jump on enemies).  
* **Level Progression:**  
  * **Levels 1-3:** Tutorial levels. Introduce movement, jumping, simple enemy patterns (Wolves), basic hazards (small gaps, water pools), collectibles (Coins, Health). Gentle slope.  
  * **Levels 4-6:** Introduce more complex platforming (moving platforms \- slow logs), new enemies (Spiders dropping), simple environmental hazards (thorn bushes \- damage on touch). Introduce first hidden areas.  
  * **Levels 7-9:** Combine platforming and combat more intensely. Introduce Archer Elves (ranged attacks), require more precise jumping, vine swinging mechanics. Checkpoints become more crucial.  
  * **Level 10:** **Boss Fight: Giant Tree Monster.** Arena designed with platforms at different heights to dodge attacks. Requires learning attack patterns (branch swings, fruit drops).  
* **Key Elements:** Trees, vines, rivers, branches, mossy platforms, water hazards, thorn bushes.  
* **Post-Boss Ability:** Double Jump Talisman unlocked.

### **4.2. Desert (Levels 11-20)**

* **Theme:** Hot, arid, mysterious ruins. Focus on environmental hazards and introduces more complex enemy types. Utilizes Double Jump.  
* **Mechanics Introduced/Tested:** Double Jump mastery, heat haze effect (visual only), shifting sands (slow movement), quicksand (hazard).  
* **Level Progression:**  
  * **Levels 11-13:** Introduce Desert theme, Scorpions (close combat threat), shifting sands. Levels designed with wider gaps requiring Double Jump.  
  * **Levels 14-16:** Introduce Sandworms (emerging hazard), Bandits (ranged threat), quicksand pits. Platforming involves crumbling ruins and navigating around hazards. More complex hidden areas using Double Jump.  
  * **Levels 17-19:** Combine all Desert elements. Require precise Double Jumps, navigating multiple enemy types simultaneously, timing movements over hazardous terrain.  
  * **Level 20:** **Boss Fight: Sand Dragon.** Large, open arena with destructible pillars for temporary cover. Boss utilizes burrowing and fire breath, requiring quick movement and Double Jumps to evade.  
* **Key Elements:** Sand dunes, cacti, ruins, quicksand, shifting sands, heat haze.  
* **Post-Boss Ability:** Wall Grab/Wall Jump Talisman unlocked.

### **4.3. Mountain (Levels 21-30)**

* **Theme:** Cold, treacherous, vertical. Focus on verticality, slippery surfaces, and new movement challenges using Wall Grab/Jump.  
* **Mechanics Introduced/Tested:** Wall Grab/Jump mastery, slippery ice surfaces, wind gusts (affect player movement), avalanches (timed hazard).  
* **Level Progression:**  
  * **Levels 21-23:** Introduce Mountain theme, Yetis (throwing projectiles), slippery ice platforms. Levels feature vertical shafts and walls specifically designed for Wall Grab/Jump practice.  
  * **Levels 24-26:** Introduce Eagles (swooping attacks), wind gusts affecting jumps, require sequences of Wall Jumps. More complex vertical layouts.  
  * **Levels 27-29:** Introduce Rock Golems (tough enemies), timed avalanche sections, combine slippery surfaces with Wall Jumps and enemy avoidance. Peak difficulty for this environment.  
  * **Level 30:** **Boss Fight: Ice Queen.** Arena with multiple icy platforms and walls. Boss summons blizzards (obscuring vision/pushing player) and ice spikes (area denial), requiring Wall Jumps and precise movement.  
* **Key Elements:** Rocky cliffs, snow, ice, wind tunnels, breakable ice walls, stalactites (falling hazards).  
* **Post-Boss Ability:** Dash Talisman unlocked.

### **4.4. Cave (Levels 31-40)**

* **Theme:** Dark, eerie, crystalline. Focus on limited visibility, precision platforming, and utilizing all unlocked abilities.  
* **Mechanics Introduced/Tested:** Dash mastery (crossing large gaps, dodging), limited visibility (requiring careful movement), crystal-based puzzles (e.g., hitting crystals to activate platforms), echo location segments (sound reveals path briefly \- conceptual).  
* **Level Progression:**  
  * **Levels 31-33:** Introduce Cave theme, Bats (erratic movement), limited light sources. Levels designed with gaps requiring Dash, introduce simple crystal activation mechanics.  
  * **Levels 34-36:** Introduce Slimes (splitting enemies), Shadow Creatures (ambush tactics), more complex crystal puzzles requiring timed dashes or jumps. Sections with near-total darkness requiring careful navigation.  
  * **Levels 37-39:** Combine all Cave elements and require mastery of Double Jump, Wall Jump, and Dash in sequence. Complex platforming challenges in low light, multiple enemy types, intricate puzzles. The ultimate test before the final boss.  
  * **Level 40:** **Final Boss Fight: Crystal Guardian.** Multi-phase fight in a large crystalline arena. Requires using all abilities to dodge beams, navigate teleporting boss patterns, and potentially hit weak points revealed by crystal mechanics.  
* **Key Elements:** Glowing crystals, darkness, stalactites/stalagmites, underground rivers, bioluminescent flora, echo flowers (conceptual).

## **5\. Level Elements Detailed**

* **Platforms:** Static, moving (horizontal, vertical, rotating), crumbling, slippery (ice), temporary (crystal-activated), invisible (revealed by mechanic).  
* **Hazards:** Spikes, pitfalls, traps (falling rocks, arrow traps, swinging axes), environmental (water, quicksand, lava streams \- conceptual), enemy projectiles.  
* **Enemies:** Placed strategically to challenge platforming (e.g., Archer Elf on a platform requiring a tricky jump), guard pathways, or create intense combat encounters. Patrol paths and attack triggers designed per enemy type.  
* **Collectibles:** Placed on main paths, tempting risk near hazards, and hidden in secret areas. Coin trails can guide players. Health items placed before challenging sections or boss fights.  
* **Checkpoints:** Placed after difficult sections, before boss encounters, and roughly midway through longer levels. Respawning restores full health but resets enemies in the immediate vicinity.

## **6\. Difficulty Curve**

The difficulty curve is designed to be steady within each environment, with noticeable ramps between environments and after acquiring new abilities. Optional hidden areas offer steeper challenges for experienced players throughout the game. The final Cave environment represents the peak difficulty, requiring mastery of all mechanics.