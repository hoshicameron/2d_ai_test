# Task ID: 4.1
# Parent Task ID: 4
# Title: Level Design Tools & Workflow Setup
# Status: pending
# Dependencies: 1.1.1 # Unity Project Setup
# Priority: high
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Set up tools and establish a workflow for level design, primarily utilizing Unity's Tilemap system, creating reusable level element prefabs, and implementing basic hazard and checkpoint systems.

# Details:
This task involves:
- Setting up Tile Palettes with Rule Tiles for efficient level geometry creation.
- Creating Prefab Palettes for reusable elements (platforms, hazards, etc.).
- Implementing basic hazard systems (spikes, pits).
- Implementing a checkpoint system.

Refer to subtasks 4.1.1 through 4.1.4.

# Acceptance Criteria:
- Tile Palettes with Rule Tiles are created and usable for level blockout.
- A collection of reusable level element prefabs is available.
- Basic hazard prefabs (spikes, pits) are functional.
- A checkpoint system is implemented and functional.
- Level designers can efficiently create and iterate on levels using these tools.

# Test Strategy:
- Create a test level using the Tilemap system and prefab palettes.
- Test functionality of hazards (player takes damage or dies).
- Test checkpoint system: player activates checkpoint, dies, and respawns at the checkpoint.

# Notes/Questions:
- "Prefab Palettes" might refer to organizing prefabs in project folders for easy drag-and-drop, or using a custom editor tool if one is built (e.g., `PetalsOfHope.Editor.LevelDesign` mentioned in plan, but no specific tool described beyond Tilemaps).