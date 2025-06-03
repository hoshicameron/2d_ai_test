# Task ID: 4.1.3
# Parent Task ID: 4.1
# Title: Implement Basic Hazard Systems (Spikes, Pits)
# Status: pending
# Dependencies: 3.1.1, 2.5 # IDamageable, PlayerHealth
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement basic hazard systems, specifically spikes and pits, as prefabs or tile logic. These hazards should damage or kill the player on contact.

# Details:
1.  **Spikes Hazard:**
    *   **Prefab Approach (Recommended for flexibility):**
        *   Create `SpikeHazard.cs` script:
            ```csharp
            // In Assets/_Project/Scripts/Gameplay/Hazards/SpikeHazard.cs
            namespace PetalsOfHope.Gameplay.Hazards
            {
                using UnityEngine;
                using PetalsOfHope.Interfaces; // For IDamageable

                public class SpikeHazard : MonoBehaviour
                {
                    [Tooltip("Damage dealt by spikes on contact. Set to high value for instant kill.")]
                    public int damageAmount = 1000; // Or player's max health for insta-kill

                    private void OnTriggerEnter2D(Collider2D collision)
                    {
                        // Could also use OnCollisionEnter2D if spikes are not triggers
                        IDamageable damageable = collision.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            // Check if colliding object is the Player (e.g., by tag or component)
                            if (collision.CompareTag("Player")) // Assuming Player GameObject has "Player" tag
                            {
                                damageable.TakeDamage(damageAmount);
                            }
                        }
                    }
                }
            }
            ```
        *   Create `SpikePrefab.prefab` in `Assets/_Project/Prefabs/LevelElements/Hazards/`.
        *   Add a sprite for spikes.
        *   Add `Collider2D` (e.g., `BoxCollider2D` or `PolygonCollider2D` matching spike shape). Set `Is Trigger` to true.
        *   Attach `SpikeHazard.cs` script. Configure `damageAmount`.
    *   **Tile Logic Approach (Alternative for simple static spikes):**
        *   Create a `SpikeTile.cs` inheriting from `UnityEngine.Tilemaps.Tile` (or `RuleTile`).
        *   Override `GetTileData` to set a specific `colliderType` if needed.
        *   Override `OnTriggerEnter2D` (requires Tilemap to have Rigidbody2D and `TilemapCollider2D` configured for individual tile collision callbacks, which can be complex) or use a global script that checks player position against spike tile positions. This is generally more complex than prefab triggers. **Prefab approach is simpler for discrete hazards.**

2.  **Pits Hazard (Death Zone):**
    *   Create `DeathZone.cs` script:
        ```csharp
        // In Assets/_Project/Scripts/Gameplay/Hazards/DeathZone.cs
        namespace PetalsOfHope.Gameplay.Hazards
        {
            using UnityEngine;
            using PetalsOfHope.Interfaces;

            public class DeathZone : MonoBehaviour
            {
                // Typically instant kill, damage amount can be very high
                public int damageAmount = 10000; 

                private void OnTriggerEnter2D(Collider2D collision)
                {
                    if (collision.CompareTag("Player"))
                    {
                        IDamageable playerDamageable = collision.GetComponent<IDamageable>();
                        if (playerDamageable != null)
                        {
                            playerDamageable.TakeDamage(damageAmount);
                        }
                        // Optionally, could directly trigger a respawn mechanism here
                        // e.g., GameManager.Instance.RespawnPlayerAtLastCheckpoint();
                    }
                    // Optionally destroy other objects that fall in, like enemies or physics props
                    // else if (ShouldDestroy(collision.gameObject)) { Destroy(collision.gameObject); }
                }
            }
        }
        ```
    *   Create `DeathZonePrefab.prefab` (or just use an empty GameObject with the script and a collider).
    *   Add a `BoxCollider2D`. Set `Is Trigger` to true. Resize it to cover the pit area.
    *   Attach `DeathZone.cs` script.
    *   Make the DeathZone visual invisible in game (e.g., no SpriteRenderer, or disable it). Gizmos can be used for editor visibility.

# Acceptance Criteria:
- `SpikePrefab` is created and functional: Player touching spikes takes damage (ideally dies if damage is high enough) from `SpikeHazard.cs`.
- `DeathZone` setup (prefab or configured GameObject) is functional: Player entering the zone's trigger collider takes massive damage / dies from `DeathZone.cs`.
- Player needs to have the "Player" tag for these hazards to specifically target them.
- PlayerHealth needs to implement `IDamageable` for these hazards to work as written.

# Test Strategy:
- Manual Testing:
    - In a test level, place `SpikePrefab` instances. Have the player character walk/fall onto them. Verify player takes damage/dies.
    - Create a pit area. Place a `DeathZone` GameObject with a large trigger collider covering the pit. Have the player fall into it. Verify player dies.
    - Ensure Player GameObject has the "Player" tag.

# Notes/Questions:
- For these hazards to deal damage, `PlayerHealth.cs` (Task 2.5) must implement `IDamageable` (Task 3.1.1). If not yet refactored, a temporary `IDamageable` component could be added to the player for testing, or `PlayerHealth.TakeDamage()` could be called directly by hazard scripts (less ideal).
- The prefab approach for spikes is generally more flexible than tile-based logic for dynamic or varied spike setups.