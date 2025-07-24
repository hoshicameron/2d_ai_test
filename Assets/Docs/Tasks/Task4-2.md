# Task ID: 4.2
# Parent Task ID: 4
# Title: Scene Management System Implementation
# Status: completed
# Dependencies: 1.2 # Event Bus (optional, for scene load requests)
# Priority: high
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement a system for managing scene loading and transitions. This includes a `SceneLoader` service, `SceneDataSO` for metadata, and a way to trigger level transitions.

# Details:
This system will handle all aspects of loading new scenes, including main menu, game levels, and potentially other utility scenes. It should support basic transitions like fade-to-black.

Namespace: `PetalsOfHope.Systems.SceneManagement`

Refer to subtasks 4.2.1, 4.2.2, and 4.2.3.

# Acceptance Criteria:
- All subtasks (4.2.1 - 4.2.3) are completed.
- Scenes can be loaded by name or build index via `SceneLoader`.
- `SceneDataSO` assets can be used to manage scene metadata and trigger loads.
- Basic scene transitions (e.g., fade-to-black) are implemented and used during scene loads.
- Level transitions (e.g., player reaching end of level trigger) are functional.

# Test Strategy:
- Create multiple simple scenes (e.g., MainMenu, Level1, Level2).
- Test `SceneLoader.LoadScene()` with scene names and build indices.
- Test using `SceneDataSO` to load scenes.
- Verify fade-to-black transition plays correctly.
- Implement a simple end-of-level trigger in one scene that loads another, and test it.

# Notes/Questions:
- Consider using Addressables for scene management if the project is expected to grow very large with many scenes, for better memory management and content updates. The plan mentions `UnityEngine.SceneManagement` or Addressables, so standard SceneManagement is the baseline.