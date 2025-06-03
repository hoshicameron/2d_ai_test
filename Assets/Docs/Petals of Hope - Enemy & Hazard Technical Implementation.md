# **Petals of Hope \- Enemy & Hazard Technical Implementation**

## **1\. Introduction**

This document provides specific technical parameters and statistics for enemies, bosses, and hazards within *Petals of Hope*. These values serve as a baseline for implementation in the Unity engine using C\#. All stats are subject to balancing during playtesting.

## **2\. Player Base Stats (Reference)**

* **Starting Health:** 3 Hearts  
* **Damage from Enemy/Hazard:** Typically 1 Heart (unless specified otherwise)  
* **Invincibility Frames:** Short period (e.g., 1-1.5 seconds) after taking damage where Elias cannot be hurt again. Visual feedback (flashing sprite) required.  
* **Defeat Method:** Jumping on most enemies' heads defeats them.

## **3\. Standard Enemy Stats**

*(Speed values are relative placeholders: Slow ≈ 1 unit/sec, Medium ≈ 2-3 units/sec, Fast ≈ 4-5 units/sec. Detection Range is the distance at which an enemy might change behavior)*

### **3.1. Forest Enemies (Levels 1-10)**

| Enemy | Health (Hits) | Damage (Hearts) | Movement Speed | Behavior Notes | Detection Range | Attack Rate | Score | Drops (Chance) |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| **Wolf** | 1 | 1 (Contact) | Medium | Patrols back and forth. Charges briefly when player is detected within range. | Medium | N/A | 10 | Coin (10%) |
| **Spider** | 1 | 1 (Contact) | N/A (Static) | Hangs from ceiling. Drops down when player passes underneath. Climbs back up. | Small (Below) | N/A | 15 | Coin (10%) |
| **Archer Elf** | 1 | 1 (Projectile) | Slow (Stationary) | Stands still. Shoots arrows horizontally when player is in line of sight. | Long (LOS) | Medium | 25 | Coin (15%) |
| *Projectile: Arrow* | N/A | 1 | Medium | Travels horizontally until hitting wall/player or off-screen. | N/A | N/A | N/A | N/A |

### **3.2. Desert Enemies (Levels 11-20)**

| Enemy | Health (Hits) | Damage (Hearts) | Movement Speed | Behavior Notes | Detection Range | Attack Rate | Score | Drops (Chance) |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| **Scorpion** | 1 | 1 (Contact/Sting) | Slow | Patrols. Briefly pauses and lunges with stinger when player is close. | Small | Slow | 20 | Coin (10%) |
| **Sandworm** | 2 | 1 (Contact) | N/A (Static) | Hidden below sand. Emerges vertically when player approaches designated spot. | Medium (Above) | N/A | 30 | Coin (20%) |
| **Bandit** | 2 | 1 (Projectile) | Medium | Patrols short distances or stays stationary. Throws knives towards player. | Medium | Medium | 35 | Coin (20%) |
| *Projectile: Knife* | N/A | 1 | Medium-Fast | Travels in a slight arc. | N/A | N/A | N/A | N/A |

### **3.3. Mountain Enemies (Levels 21-30)**

| Enemy | Health (Hits) | Damage (Hearts) | Movement Speed | Behavior Notes | Detection Range | Attack Rate | Score | Drops (Chance) |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| **Yeti** | 3 | 1 (Contact/Projectile) | Slow | Patrols. Stops and throws snowballs towards player when detected. High knockback. | Medium | Slow | 40 | Coin (25%) |
| **Eagle** | 1 | 1 (Contact) | Fast | Flies overhead. Swoops down diagonally towards player when detected below. | Large (Below) | Medium | 30 | Coin (15%) |
| **Rock Golem** | 4 | 1 (Contact) | Very Slow | Patrols slowly. High resistance/knockback immunity. Cannot be stunned easily. | N/A | N/A | 50 | Coin (30%) |
| *Projectile: Snowball* | N/A | 1 | Medium | Travels in an arc. | N/A | N/A | N/A | N/A |

### **3.4. Cave Enemies (Levels 31-40)**

| Enemy | Health (Hits) | Damage (Hearts) | Movement Speed | Behavior Notes | Detection Range | Attack Rate | Score | Drops (Chance) |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| **Bat** | 1 | 1 (Contact) | Medium | Flies in erratic patterns (e.g., figure-eight, sine wave) within a defined area. | N/A | N/A | 25 | Coin (15%) |
| **Slime** | 1 | 1 (Contact) | Slow | Hops slowly. Splits into two smaller slimes upon defeat. | N/A | N/A | 15 | Coin (10%) |
| **Small Slime** | 1 | 1 (Contact) | Slow | Hops slowly. Defeated normally. | N/A | N/A | 10 | Coin (5%) |
| **Shadow Creature** | 2 | 1 (Contact) | Fast | Stays hidden in dark areas/shadows. Dashes out quickly when player gets close. | Small | N/A | 45 | Coin (25%) |

## **4\. Boss Stats**

*Boss health should be significantly higher and potentially represented by a dedicated boss health bar UI element. Damage values might be higher or attacks more frequent.*

| Boss | Environment | Est. Health (Hits) | Damage (Hearts) | Key Attacks / Behavior | Score | Drops |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| **Giant Tree Monster** | Forest | 10 | 1 | Sweeping branch attacks (low/high), drops explosive fruit (area denial), vulnerable point revealed periodically. | 250 | Double Jump Talisman |
| **Sand Dragon** | Desert | 15 | 1 | Breathes fire horizontally, burrows underground and emerges elsewhere, tail whip (close range). | 500 | Wall Jump Talisman |
| **Ice Queen** | Mountain | 20 | 1-2 | Summons blizzard (pushes player/obscures view), shoots ice spikes (ground/falling), teleports short distances. | 750 | Dash Talisman |
| **Crystal Guardian** | Cave | 25 | 1-2 | Shoots focused energy beams (tracking/static), teleports across arena, summons crystal shards (projectiles), shield phase. | 1000 | Game End Trigger |

## **5\. Hazard Stats**

| Hazard | Damage (Hearts) | Behavior Notes |
| :---- | :---- | :---- |
| **Spikes** | 1 or Instant Death | Static hazard. Damage on contact. |
| **Pitfalls** | Instant Death | Falling off-screen or into designated pit areas. |
| **Water (Deep)** | 1 (or gradual drain) | Player sinks, takes damage, respawns nearby. |
| **Quicksand** | N/A (Slows) | Impedes movement significantly. |
| **Thorn Bush** | 1 | Damage on contact. |
| **Falling Rocks** | 1 | Timed trap, falls when player passes under/near. |
| **Arrow Traps** | 1 | Shoots arrows periodically or when triggered. |
| **Swinging Axes** | 1 | Moves back and forth on a fixed arc. |
| **Lava (Conceptual)** | Instant Death | Standard high-damage environmental hazard. |
| **Ice Surface** | N/A (Slippery) | Reduced friction, affects player control. |
| **Wind Gusts** | N/A (Push) | Applies force to the player horizontally/vertically. |

## **6\. Implementation Notes (Unity/C\#)**

* **Enemy Prefabs:** Create prefabs for each enemy type containing sprites, colliders (for physics and damage detection), and scripts.  
* **Enemy Script:** A base EnemyController.cs script could handle common logic like health, taking damage, death effects (particles, sound), score awarding, and basic movement.  
* **Derived Scripts:** Specific enemy types (e.g., WolfController.cs, ArcherElfController.cs) inherit from EnemyController and implement unique behaviors (charging, shooting, dropping).  
* **Scriptable Objects:** Consider using Scriptable Objects to store enemy stats (Health, Damage, Speed, Score, etc.). This allows easy tweaking and balancing without modifying code directly. Prefabs can reference these Scriptable Objects.  
* **State Machines:** Use a simple state machine (e.g., Idle, Patrol, Chase, Attack, Hurt, Die) within enemy scripts to manage behavior logic.  
* **Physics Layers:** Utilize Unity's physics layers to manage collisions (e.g., player vs. enemy, player projectile vs. enemy, enemy vs. environment).  
* **Damage Handling:** Player script should handle taking damage, checking for invincibility frames, and updating health UI. Enemies trigger damage via collision or projectile hit detection. Player attack script handles dealing damage to enemies (e.g., detecting jump on top collider).  
* **Pooling:** For frequently spawned elements like projectiles or simple enemies, consider using object pooling to improve performance.

This document provides the initial data needed. Remember to continuously test and adjust these values throughout development to achieve the desired difficulty curve and game feel.