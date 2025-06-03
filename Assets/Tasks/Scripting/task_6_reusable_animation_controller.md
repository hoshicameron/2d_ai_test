# Task ID: 6
# Parent Task ID: None
# Title: Reusable Animation Controller
# Status: completed
# Dependencies: 1, 5
# Priority: high
# Estimated Effort: S
# Assignee: AI

# Description:
Implement a reusable Animation Controller to manage animations.

# Details:
1. Implement `AnimationController.cs` (MonoBehaviour) in `_Project/Scripts/Core/Animation/`.
2. Requires `Animator` component.
3. Caches `Animator` reference.
4. Public methods: `Play(string/int stateNameOrHash)`, `SetBool(string/int param, bool val)`, `SetFloat(string/int param, float val)`, `SetInteger(string/int param, int val)`, `SetTrigger(string/int param)`.
5. Decouples animation calls from state machines/controllers.

# Acceptance Criteria:
- `AnimationController.cs` is implemented correctly.
- Animation methods are functional and decouple animation logic.

# Test Strategy:
- Manual testing of animation control in the Unity Editor.
- Verify that animations are triggered correctly.

# Notes/Questions:
- Ensure that the Animation Controller is reusable across different game objects.
- Verify that the Animator component is properly cached.
