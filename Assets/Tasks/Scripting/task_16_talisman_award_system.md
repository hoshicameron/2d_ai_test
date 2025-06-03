# Task ID: 16
# Parent Task ID: None
# Title: Talisman Award System
# Status: completed
# Dependencies: 15
# Priority: high
# Estimated Effort: S
# Assignee: AI

# Description:
Implement the Talisman Award System.

# Details:
1. Implement `TalismanDataSO.cs` (`ScriptableObject`) in `_Project/Data/Collectibles/Talismans/`.
2. Implement `InventorySystem.cs` in `_Project/Scripts/Systems/Inventory/`.
3. Integrate with Event Bus to raise `TalismanAwardedEventSO`.

# Acceptance Criteria:
- `TalismanDataSO` is implemented correctly.
- `InventorySystem` is functional and manages talisman data.
- Integration with Event Bus is correct.

# Test Strategy:
- Manual testing of talisman awarding in the Unity Editor.
- Verify that talisman data is correctly stored and events are raised.

# Notes/Questions:
- Ensure that talisman awarding logic is correctly implemented.
- Verify that the Inventory System is properly integrated with the Event Bus.
