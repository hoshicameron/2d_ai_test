# Task ID: 9
# Parent Task ID: None
# Title: Player Health
# Status: completed
# Dependencies: 8
# Priority: high
# Estimated Effort: S
# Assignee: AI

# Description:
Implement the Player Health system.

# Details:
1. Implement `PlayerHealth.cs` (MonoBehaviour) in `_Project/Scripts/Gameplay/Player/`.
2. Fields: `PlayerStatsSO`, `playerDiedEvent`, `playerHealthChangedEvent`, `_currentHealth`.
3. Initialize health, handle damage, and die logic.

# Acceptance Criteria:
- `PlayerHealth` is implemented correctly.
- Health is initialized and updated correctly.
- Damage handling and die logic are functional.

# Test Strategy:
- Unit tests for health logic.
- Manual testing of health changes and death in the Unity Editor.

# Notes/Questions:
- Ensure that health changes are correctly raised as events.
- Verify that death logic is correctly triggered.
