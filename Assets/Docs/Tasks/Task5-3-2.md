# Task ID: 5.3.2
# Parent Task ID: 5.3
# Title: Create VFX Prefabs for Key Actions
# Status: pending
# Dependencies: None (requires particle art/textures, or use simple Unity particles)
# Priority: medium
# Estimated Effort: L (art/design heavy)
# Assignee: Unassigned

# Description:
Create particle effect prefabs for key game actions using Unity's Particle System (Shuriken). Examples include jump dust, landing impact, talisman collect, hit sparks, and enemy death effects.

# Details:
1.  **Identify Key Actions for VFX:**
    *   Player Jump (dust puff at feet)
    *   Player Land (small impact dust/lines)
    *   Talisman Collect (sparkle, glow)
    *   Player/Enemy Hit (sparks, blood (if applicable), impact flash)
    *   Enemy Death (smoke puff, disintegration, explosion - type dependent)
    *   Player Dash (trail, start/end burst)
    *   Wall Slide (dust/sparks from wall)
    *   Weapon swings/impacts (if any)
2.  **Design and Create Particle Systems:**
    *   For each identified effect:
        *   Create a new Particle System GameObject (`GameObject > Effects > Particle System`).
        *   Configure its modules (Emission, Shape, Velocity over Lifetime, Color over Lifetime, Size over Lifetime, Renderer, etc.) to achieve the desired look.
        *   Use appropriate materials and textures (can be simple for placeholders, e.g., default particle material, or custom spritesheet animations).
        *   Ensure "Play On Awake" is **unchecked** if these will be pooled/triggered by script.
        *   Set "Stop Action" to "Disable" or "Destroy" (if not pooled) or "Callback" (if system handles it). If using `VFXManager` from 5.3.1, its auto-return relies on duration, so "Disable" is good.
3.  **Create Prefabs:**
    *   Save each configured Particle System GameObject as a prefab.
    *   Location: `Assets/_Project/Prefabs/VFX/` (with subfolders like `Player/`, `Enemy/`, `Environment/`, `Items/`).
    *   Examples:
        *   `JumpDust_VFX.prefab`
        *   `LandingImpact_VFX.prefab`
        *   `TalismanCollect_VFX.prefab`
        *   `HitSparks_VFX.prefab`
        *   `EnemyDeath_Puff_VFX.prefab`
4.  **(If using `VFXManager`) Register Prefabs:**
    *   Add these prefabs to the `VFXManager.initialPools` list with appropriate names and initial pool sizes.

# Acceptance Criteria:
- At least 3-5 distinct VFX prefabs for key actions (e.g., jump dust, landing impact, hit sparks, basic enemy death) are created.
- Particle systems are configured to produce visually recognizable (even if placeholder) effects.
- Prefabs are stored in the designated project folder.
- Prefabs are configured not to "Play On Awake" and have a suitable "Stop Action" if they are to be pooled or managed by `VFXManager`.

# Test Strategy:
- Manual Verification:
    - Drag each VFX prefab into an empty scene and manually trigger `Play()` on its `ParticleSystem` component in the Inspector (or via a test script).
    - Observe the effect to ensure it looks and behaves as intended (duration, spread, color, etc.).
    - Check console for any errors related to materials or shaders.

# Notes/Questions:
- This task can be very art-dependent. If art resources are limited, use simple Unity default particles or basic shapes.
- Focus on clarity of feedback first, then visual polish.
- Keep performance in mind: avoid excessive particle counts, overdraw, or complex shaders unless optimized.
- Shader Graph can be used for more custom particle appearances or screen effects, but Shuriken is the primary tool here.