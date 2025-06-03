# Task ID: 5.3.3
# Parent Task ID: 5.3
# Title: Trigger VFX from Gameplay and Implement Screen Effects
# Status: pending
# Dependencies: 5.3.1 (VFXManager if used), 5.3.2 (VFX Prefabs), various gameplay components
# Priority: medium
# Estimated Effort: L (spread across many integrations)
# Assignee: Unassigned

# Description:
Integrate the created VFX prefabs into gameplay by triggering them from relevant components (e.g., `PlayerController`, `EnemyBase`, `InventorySystem`) or via events. Implement basic screen effects like camera shake or flash if desired.

# Details:
1.  **Triggering VFX from Gameplay Scripts:**
    *   Identify where VFX should be triggered.
    *   In the corresponding scripts, get a reference to `VFXManager.Instance` (if used) or have direct `GameObject` fields for specific VFX prefabs.
    *   Call `VFXManager.Instance.PlayEffect("VFXName", position, rotation)` or `Instantiate(vfxPrefab, position, rotation)`.
    *   Examples:
        *   **Player Land:** In `PlayerController.CheckIfGrounded()` when `!wasGrounded && IsGrounded`:
            `VFXManager.Instance.PlayEffect("LandingImpact", _groundCheckPoint.position, Quaternion.identity);`
        *   **Player Jump:** In `JumpingState.Enter()`:
            `VFXManager.Instance.PlayEffect("JumpDust", _player.transform.position, Quaternion.identity);`
        *   **Enemy Taking Damage:** In `EnemyBase.OnDamaged()`:
            `VFXManager.Instance.PlayEffect("HitSparks", hitLocation, Quaternion.identity);` (hitLocation would need to be passed or estimated)
        *   **Enemy Death:** In `EnemyBase.HandleDeathVisualsAndCleanup()`:
            `VFXManager.Instance.PlayEffect("EnemyDeathPuff", transform.position, Quaternion.identity);`
        *   **Talisman Collect:** In `InventorySystem.AddTalisman()` (or the script that calls it):
            `VFXManager.Instance.PlayEffect("TalismanCollect", itemPosition, Quaternion.identity);`
2.  **Implement Screen Effects (Basic):**
    *   **Camera Shake:**
        *   Could be a method in `CinemachineImpulseSource` (if using Cinemachine and its impulse system) or a simple script on the main camera.
        *   Example `CameraShake.cs` (simple version on Main Camera):
            ```csharp
            // public class CameraShake : MonoBehaviour {
            //    public IEnumerator Shake(float duration, float magnitude) { /* ... logic to offset camera pos ... */ }
            // }
            // Triggered by: Camera.main.GetComponent<CameraShake>()?.StartCoroutine(Shake(0.1f, 0.2f));
            ```
        *   Cinemachine Impulse is generally preferred for more robust and controllable camera shake. Add `CinemachineImpulseSource` to objects that cause shake (e.g., player landing hard, enemy exploding) and `CinemachineImpulseListener` on the Virtual Camera.
    *   **Screen Flash:**
        *   Use a UI Image that covers the screen, quickly fade its alpha in and out.
        *   Could be a method in `UIManager` or a dedicated `ScreenFlash.cs`.
        *   Trigger for impacts, explosions, or special ability activations.

3.  **Triggering Screen Effects:**
    *   Call camera shake/flash methods at appropriate impact points (e.g., player taking heavy damage, large enemy landing, explosions).

# Acceptance Criteria:
- VFX are triggered for key player actions (jump, land, dash).
- VFX are triggered for enemy actions (damage, death).
- VFX are triggered for significant game events (e.g., talisman collection).
- (If implemented) Basic camera shake or screen flash occurs for impactful events.
- VFX are positioned and rotated correctly.
- VFX triggering does not cause noticeable performance drops.

# Test Strategy:
- Manual Playtesting:
    - Perform all actions that should trigger VFX. Verify effects play correctly and at the right time/location.
    - Test screen effects: ensure they are not too jarring or disorienting.
    - Monitor performance (frame rate) when many VFX are active, especially if not using pooling or if effects are complex.

# Notes/Questions:
- VFX triggering points are numerous. This task involves many small additions to existing scripts.
- The "hitLocation" for `HitSparks` might require more complex collision data (e.g., from `OnCollisionEnter2D` `contactPoint`).
- Cinemachine Impulse system is powerful for camera shake and recommended if Cinemachine is already in use for the main camera.
- Ensure VFX transform parenting is handled correctly if effects should follow moving objects. `VFXManager.PlayEffect` has an optional parent parameter.