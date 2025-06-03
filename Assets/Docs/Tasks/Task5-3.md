# Task ID: 5.3
# Parent Task ID: 5
# Title: Visual Effects (VFX) Implementation
# Status: pending
# Dependencies: Potentially AnimationController (for triggering VFX via anim events), Event Bus
# Priority: medium # Polish item, but important for feel
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement visual effects using Unity's Particle System (Shuriken) and potentially Shader Graph for custom effects. This includes creating an optional `VFXManager` for pooling and managing common effects, creating effect prefabs, and triggering them from gameplay.

# Details:
This system will add visual flair and feedback to game actions and events.
- Utilize Unity's Particle System.
- (Optional) `VFXManager.cs` for pooling common VFX prefabs.
- Create prefabs for various effects (jump dust, landing impact, talisman collect, hit sparks, enemy death).
- Trigger effects from gameplay components or events.
- Implement screen effects (camera shake, flash) if desired.

Refer to subtasks 5.3.1, 5.3.2, and 5.3.3.

# Acceptance Criteria:
- All subtasks (5.3.1 - 5.3.3) are completed.
- Particle effect prefabs for key actions (e.g., jump, land, hit, death) are created.
- (If `VFXManager` implemented) VFX are instantiated and pooled effectively.
- VFX are triggered at appropriate moments in gameplay (player actions, enemy actions, events).
- (If implemented) Basic screen effects like camera shake enhance impact.
- Visual effects contribute positively to game feel without performance degradation.

# Test Strategy:
- Playtest game actions and verify corresponding VFX trigger correctly.
- Observe visual quality and timing of effects.
- If using pooling, verify VFX are reused and not excessively instantiated/destroyed.
- Test screen effects for appropriate intensity and context.

# Notes/Questions:
- This phase relies on having art assets (spritesheets for particles, textures) or creating simple procedural particles.
- Performance of particle effects is a key consideration, especially for mobile or lower-end platforms. Object pooling (Task 5.4) is crucial for VFX.