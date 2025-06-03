# Task ID: 4.3
# Parent Task ID: 4
# Title: Talisman Award System Implementation
# Status: pending
# Dependencies: 1.2, 1.3 # Event Bus, ScriptableObject Data Management (for TalismanDataSO)
# Priority: high
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement a system for awarding talismans to the player. This includes defining `TalismanDataSO`, an `InventorySystem` to manage collected talismans, and a UI notification mechanism (basic for now, full UI in Phase 5).

# Details:
This system allows the player to collect key items (Talismans) that may unlock abilities or signify progression.
- `TalismanDataSO`: A ScriptableObject to define talisman properties.
- Award Mechanism: Talismans awarded programmatically (e.g., after boss defeat via event).
- `InventorySystem.cs`: Manages the list of collected talismans.
- UI Notification: `UIManager` (from Phase 5, or a placeholder here) listens to `TalismanAwardedEventSO` to display a notification.

Namespace for Inventory: `PetalsOfHope.Systems.Inventory`

Refer to subtasks 4.3.1, 4.3.2, 4.3.3.

# Acceptance Criteria:
- All subtasks (4.3.1 - 4.3.3) are completed.
- `TalismanDataSO` assets can define unique talismans.
- `InventorySystem` can add talismans to a persistent collection.
- Awarding a talisman raises `TalismanAwardedEventSO`.
- A basic notification (e.g., debug log or simple on-screen text) appears when a talisman is awarded.
- (Later) Collected talismans are saved and loaded (part of Task 3.4.5 via `InventorySystem` implementing `ISaveable`).

# Test Strategy:
- Create `TalismanDataSO` assets.
- Manually trigger talisman award (e.g., via a test script or after a mock "boss defeated" event).
- Verify `InventorySystem` updates its collection.
- Verify `TalismanAwardedEventSO` is raised and the basic notification appears.
- Verify talisman data is correctly saved/loaded when `InventorySystem` is made `ISaveable`.

# Notes/Questions:
- The actual unlocking of abilities based on talismans will be handled by `GameProgressionManager` (Task 3.4.5) and `PlayerAbilities` (Task 3.4.4).