# Task ID: 5
# Parent Task ID: None
# Title: Input System
# Status: completed
# Dependencies: 1
# Priority: critical
# Estimated Effort: M
# Assignee: AI

# Description:
Implement the Input System using Unity's new Input System package.

# Details:
1. Create `PlayerInputActions.inputactions` asset in `_Project/Settings/Input/`.
2. Define Action Maps: `Gameplay`, `UI`.
3. Define Actions: `Move`, `Jump`, `Dash`, `Interact` for Gameplay; `Navigate`, `Submit`, `Cancel` for UI.
4. Generate C# class for the input asset.
5. Implement `InputReader.cs` (`ScriptableObject`) in `_Project/Scripts/Core/Input/`.
6. Create `InputReader` SO asset in `_Project/ScriptableObjects/Input/` and link event assets.
7. Test input with keyboard and gamepad.

# Acceptance Criteria:
- `PlayerInputActions.inputactions` asset is created and configured correctly.
- `InputReader.cs` is implemented and functional.
- Input is tested and works with both keyboard and gamepad.

# Test Strategy:
- Manual testing of input actions in the Unity Editor.
- Verify that input events are raised correctly.

# Notes/Questions:
- Ensure that the Input System is properly configured for both gameplay and UI interactions.
- Verify that input handling is responsive and accurate.
