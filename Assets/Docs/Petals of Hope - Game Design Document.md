# Petals of Hope - Game Design Document

## 1. Introduction

### Story
In the peaceful village of Greenwood, Elias lives a simple life with his daughter, Lily. One day, Lily falls ill with a mysterious disease that no healer can cure. Desperate, Elias learns of a legendary magical flower said to heal all diseases, growing deep within a dark cave beyond the mountains. Determined to save Lily, he embarks on an adventure through a dense forest, a scorching desert, and treacherous mountains to reach the cave, retrieve the flower, and return home to heal his daughter. Along the way, monsters attack him, and he must overcome platforming challenges to succeed.

## 2. Gameplay Mechanics

### Controls
- **Left/Right Arrow**: Move Elias left or right.
- **Up Arrow**: Jump.
- **Spacebar**: Perform special move (once unlocked).

### Movement
- **Walking**: Elias moves at a moderate speed.
- **Jumping**: Allows Elias to reach higher platforms or avoid obstacles.
- **Special Moves**:
  - **Double Jump**: Perform a second jump in mid-air. Unlocked after the forest boss.
  - **Wall Grab/Wall Jump**: Cling to walls and jump off them. Unlocked after the desert boss.
  - **Dash**: A quick horizontal burst of speed. Unlocked after the mountain boss.

### Health
- Elias has a health bar with a set number of hit points.
- Damage from enemies or hazards reduces health.
- Health items collected in levels restore a portion of health.

### Talismans
- Special items that permanently unlock special moves.
- Found after defeating bosses:
  - Double Jump Talisman: After forest boss (Level 10).
  - Wall Jump Talisman: After desert boss (Level 20), includes wall grab ability.
  - Dash Talisman: After mountain boss (Level 30).

### Combat
- Elias defeats enemies by jumping on their heads.
- Some enemies require multiple hits or specific timing.
- Bosses have unique attack patterns to learn and counter.

### Checkpoints
- Levels include checkpoints where Elias respawns with full health if he dies.

## 3. Level Design

### Structure
- **Total Levels**: 40, divided into four environments:
  - **Forest**: Levels 1-10.
  - **Desert**: Levels 11-20.
  - **Mountain**: Levels 21-30.
  - **Cave**: Levels 31-40.
- Each environment ends with a boss fight (Levels 10, 20, 30, 40).

### Progression
- Levels follow a linear path from start to finish.
- Hidden areas offer optional challenges with rewards like health and coins.

### Environment Themes
- **Forest**: Lush greenery, trees, vines, rivers. Platforming includes branch-jumping and water hazards.
- **Desert**: Sandy dunes, cacti, ruins. Features heat effects and shifting sands.
- **Mountain**: Rocky cliffs, snow, ice. Includes slippery surfaces and wind gusts.
- **Cave**: Dark, eerie, with glowing crystals. Limited visibility adds challenge.

### Level Elements
- **Platforms**: Static and moving, requiring timing.
- **Hazards**: Spikes, pitfalls, traps, hanging axes.
- **Enemies**: Positioned to test platforming skills.
- **Collectibles**: Coins and health items scattered throughout.

## 4. Enemies and Obstacles

### Enemies
- **Forest**:
  - Wolves: Charge at Elias.
  - Spiders: Drop from above.
  - Archer Elves: Shoot projectiles.
- **Desert**:
  - Scorpions: Sting when close.
  - Sandworms: Emerge suddenly.
  - Bandits: Throw knives.
- **Mountain**:
  - Yetis: Throw snowballs.
  - Eagles: Swoop down.
  - Rock Golems: Tough to defeat.
- **Cave**:
  - Bats: Fly erratically.
  - Slimes: Split when hit.
  - Shadow Creatures: Ambush in darkness.
- Enemy types include patrolling (move back and forth) and stationary (shoot or attack when Elias is near).

### Bosses
- **Forest Boss**: Giant Tree Monster - Swings branches, drops explosive fruit.
- **Desert Boss**: Sand Dragon - Breathes fire, burrows underground.
- **Mountain Boss**: Ice Queen - Summons blizzards and ice spikes.
- **Cave Boss**: Crystal Guardian - Shoots beams, teleports.

### Obstacles
- **Spikes**: Cause damage or instant death.
- **Moving Platforms**: Require precise timing.
- **Pitfalls**: Falling results in death.
- **Traps**: Falling rocks, arrow traps, swinging axes.

## 5. Collectibles and Power-ups

### Collectibles
- **Coins**: Scattered in levels, contribute to score or bonuses (e.g., 100 coins = extra life).
- **Health Items**: Restore health when collected.
- **Talismans**: Unlock special moves (see Talismans section above).

### Power-ups
- Power-ups refer to the talismans that enable double jump, wall jump, and dash. No temporary power-ups are included unless specified otherwise.

## 6. User Interface

### Main Menu
- Start Game
- Options (volume, controls)
- Exit

### HUD
- **Health Bar**: Displays current health.
- **Coin Counter**: Shows collected coins.
- **Ability Icons**: Indicate unlocked special moves.

### Pause Menu
- Resume
- Restart Level
- Quit to Main Menu

## 7. Design Notes
- **Target Audience**: Casual gamers, ages 10+. The game features a gentle learning curve, with early levels introducing mechanics and later levels offering optional challenges via hidden areas.
- **Platform**: Designed for PC, adaptable to mobile with control adjustments.
- **Art and Sound**: Detailed in separate Art Style Guide and Sound Design Overview documents. Expect vibrant visuals and atmospheric audio tailored to each environment.