# Task ID: 10
# Parent Task ID: None
# Title: Camera System
# Status: pending
# Dependencies: 8
# Priority: high
# Estimated Effort: S
# Assignee: AI

# Description:
Implement the Camera System to follow the player.

# Details:
1. Using Cinemachine: Add `Cinemachine Virtual Camera`, set `Follow` to Player.
2. Configure `Body` (e.g., `Framing Transposer`) for smooth follow.
3. Add `Cinemachine Confiner` with a `PolygonCollider2D` for boundaries if needed.

# Acceptance Criteria:
- Camera follows the player smoothly.
- Camera boundaries are correctly configured if using Cinemachine Confiner.

# Test Strategy:
- Manual testing of camera movement in the Unity Editor.
- Verify that the camera follows the player correctly.

# Notes/Questions:
- Ensure that Cinemachine is properly installed and configured.
- Verify that the camera boundaries are correctly set up if needed.
