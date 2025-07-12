# Task ID: 4.1.2
# Parent Task ID: 4.1
# Title: Create Prefab Palettes for Reusable Level Elements
# Status: completed
# Dependencies: 1.1.2 # Project Folder Structure
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Organize and create a collection of reusable level elements as Prefabs. These prefabs will be used alongside Tilemaps for populating levels with interactive objects, platforms, hazards, and enemy spawn points.

# Details:
1.  **Identify Reusable Elements:**
    *   Based on GDD and level design concepts, list common reusable elements. Examples:
        *   Moving platforms (various types: horizontal, vertical, path-based).
        *   Bouncing platforms/surfaces (e.g., trampolines).
        *   Breakable blocks/walls.
        *   Doors and keys/switches.
        *   Collectible items (if not handled by a different system spawning them).
        *   Specific hazard types not easily done with tiles (e.g., crushers, laser beams).
        *   Enemy spawn point markers/triggers.
        *   Light sources, decorative props.
2.  **Implement Basic Functionality for Each Element Type:**
    *   For each identified element, create a basic script if it has behavior.
    *   Example: `MovingPlatform.cs`
        ```csharp
        // Example MovingPlatform.cs
        // namespace PetalsOfHope.Gameplay.LevelElements
        // {
        //     public class MovingPlatform : MonoBehaviour
        //     {
        //         public Vector3 endPositionOffset;
        //         public float speed = 2f;
        //         public float waitTimeAtEnds = 1f;
        //         // ... logic to move between start and endPositionOffset ...
        //     }
        // }
        ```
    *   Scripts should be placed in `Assets/_Project/Scripts/Gameplay/LevelElements/` or similar.
3.  **Create Prefabs:**
    *   For each element, create a GameObject in a temporary scene, configure its components (SpriteRenderer, Collider2D, Rigidbody2D (if needed), custom script).
    *   Save these configured GameObjects as Prefabs in `Assets/_Project/Prefabs/LevelElements/` subfolders (e.g., `Platforms`, `Hazards`, `Interactables`).
    *   Examples:
        *   `HorizontalMovingPlatformPrefab`
        *   `VerticalMovingPlatformPrefab`
        *   `SpikeTrapPrefab` (see Task 4.1.3)
        *   `EnemySpawnPointPrefab` (could be an empty GameObject with a script or just a Gizmo).
4.  **Organize Prefabs for Easy Access:**
    *   Use clear naming conventions.
    *   Use subfolders within `Assets/PetalOfHope/Prefabs/LevelElements/`.
    *   (Optional "Prefab Palette" Tool) If the plan's mention of "Prefab Palettes" implies a custom editor window for quick drag-and-drop, this tool would be developed under `PetalsOfHope.Editor.LevelDesign`. For now, assume it means well-organized project folders.
5.  **Ensure Prefabs are Scalable and Configurable:**
    *   Expose key parameters as public fields on scripts (e.g., `speed`, `moveDistance` for a moving platform) so designers can customize instances.

# Acceptance Criteria:
- A collection of at least 3-5 different reusable level element prefabs (e.g., moving platform, basic interactive object, enemy spawn point marker) are created and stored in `Assets/_Project/Prefabs/LevelElements/`.
- Prefabs have necessary visuals (placeholders are fine), colliders, and basic functional scripts if applicable.
- Parameters of these prefabs are easily configurable in the Inspector.
- Level designers can drag these prefabs into scenes to build out levels.

# Test Strategy:
- Manual Verification:
    - Drag each prefab into a test scene.
    - Verify its default appearance and configuration.
    - If it has behavior (e.g., moving platform), enter Play Mode and verify it functions as expected.
    - Test customizing exposed parameters on an instance of the prefab and see if it behaves differently.

# Notes/Questions:
- This task focuses on creating the *prefabs*. The detailed implementation of complex behaviors (e.g., very intricate moving platforms, doors with key systems) might be broken into further subtasks if they become too large.
- The term "Prefab Palettes" from the plan is interpreted here as a well-organized folder structure. If a custom editor tool is intended, that's a separate, larger task.