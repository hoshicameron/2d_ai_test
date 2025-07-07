# Task ID: 2.6
# Parent Task ID: 2
# Title: Camera System Implementation
# Status: completed
# Dependencies: 2.4, 1.1.3 # PlayerController, Cinemachine package
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement the camera system, primarily using Cinemachine for smooth player following. This includes setting up a Virtual Camera and optionally a Confiner for boundaries. A manual implementation is a fallback.

# Details:
**Using Cinemachine (Recommended):**

1.  **Setup Cinemachine Virtual Camera:**
    *   Ensure Cinemachine package is installed (Task 1.1.3).
    *   In your game scene, right-click in Hierarchy -> `Cinemachine > Virtual Camera`.
    *   Rename it to `CM_PlayerFollowCam` (or similar).
    *   **`Follow` Target:**
        *   Drag the Player GameObject (the one with `PlayerController`) to the `Follow` field of the `CM_PlayerFollowCam`.
    *   **Configure `Body` Settings:**
        *   By default, `Body` might be `Transposer`. Change to `Framing Transposer` for more robust 2D follow behavior and look-ahead options.
        *   **`Framing Transposer` Settings (Example):**
            *   `Tracked Object Offset`: Adjust Y for player height (e.g., (0, 1, 0) if player pivot is at feet).
            *   `Lookahead Time`: Small value (e.g., 0.1 - 0.3) for subtle camera lead.
            *   `Lookahead Smoothing`: Adjust for responsiveness.
            *   `XDamping`, `YDamping`, `ZDamping`: Control how quickly the camera follows. Start with values around 1-2 for X and Y. Z Damping isn't usually critical for 2D.
            *   `Screen X`, `Screen Y`: Position of the follow target on screen. (0.5, 0.5) is center.
            *   `Dead Zone Width/Height`: Area where player can move without camera moving. Often 0 for tight follow, or small values for some "give".
            *   `Soft Zone Width/Height`: Area where camera starts to catch up smoothly.
    *   **Lens Settings:**
        *   Ensure `Orthographic Size` is appropriate for your game's pixel art scale and desired view.
2.  **(Optional) Setup Cinemachine Confiner:**
    *   If you need to restrict the camera to level boundaries:
    *   Create an empty GameObject named `_LevelBoundariesConfiner`.
    *   Add a `PolygonCollider2D` component to it. Make sure `Is Trigger` is checked.
    *   Edit the `PolygonCollider2D` shape to define the camera's allowed area for the level.
    *   On the `CM_PlayerFollowCam`, add the `CinemachineConfiner` extension:
        *   `Extensions > Add Extension > CinemachineConfiner`.
    *   **`Bounding Shape 2D`:** Drag the `_LevelBoundariesConfiner` (with its `PolygonCollider2D`) to this field.
    *   `Confine Mode`: Typically `Confine2D`.
    *   Adjust `Damping` if needed for smoother boundary interactions.
3.  **Ensure Main Camera has CinemachineBrain:**
    *   The main Unity Camera in the scene should automatically get a `CinemachineBrain` component when a Virtual Camera is added. Verify this. This brain selects which Virtual Camera is active.

**Manual Implementation (Alternative - if Cinemachine is not used):**

1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Camera/CameraController.cs` (if this path is chosen)
2.  **Namespace:** `PetalsOfHope.Gameplay.Camera`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Camera/CameraController.cs
    namespace PetalsOfHope.Gameplay.Camera
    {
        using UnityEngine;

        public class CameraController : MonoBehaviour
        {
            [Tooltip("The target the camera will follow (the Player).")]
            public Transform target;

            [Tooltip("How quickly the camera catches up to the target.")]
            [SerializeField] private float smoothSpeed = 0.125f;
            [Tooltip("Offset from the target's position.")]
            [SerializeField] private Vector3 offset = new Vector3(0, 1, -10); // Z = -10 for typical 2D camera

            [Header("Camera Bounds (Optional)")]
            [SerializeField] private bool useBounds = false;
            [SerializeField] private Vector2 minBounds; // Min X, Min Y
            [SerializeField] private Vector2 maxBounds; // Max X, Max Y

            private Camera _mainCamera;
            private float _cameraHalfWidth;
            private float _cameraHalfHeight;

            private void Start()
            {
                _mainCamera = GetComponent<Camera>();
                if (_mainCamera == null) {
                    Debug.LogError("CameraController script must be attached to a Camera GameObject.", this);
                    enabled = false;
                    return;
                }

                if (target == null)
                {
                    // Try to find Player dynamically if not set - good for robustness
                    GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // Requires Player to have "Player" tag
                    if (playerObj != null) target = playerObj.transform;
                    else Debug.LogWarning("Camera target (Player) not set and not found by tag.", this);
                }
                
                if (_mainCamera.orthographic)
                {
                    _cameraHalfHeight = _mainCamera.orthographicSize;
                    _cameraHalfWidth = _cameraHalfHeight * _mainCamera.aspect;
                }
            }

            private void LateUpdate()
            {
                if (target == null) return;

                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); // Adjust to Time.deltaTime for frame rate independence

                if (useBounds)
                {
                    // Adjust min/max bounds to account for camera size if bounds define world edges
                    float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x + _cameraHalfWidth, maxBounds.x - _cameraHalfWidth);
                    float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y + _cameraHalfHeight, maxBounds.y - _cameraHalfHeight);
                    transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
                }
                else
                {
                    transform.position = smoothedPosition;
                }
            }

            // Optional: Method to update bounds if they change dynamically
            public void SetBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
            {
                minBounds = newMinBounds;
                maxBounds = newMaxBounds;
                useBounds = true;
            }
        }
    }
    ```

# Acceptance Criteria:
- **Cinemachine Approach:**
    - A `Cinemachine Virtual Camera` is set up to follow the Player GameObject.
    - Camera movement is smooth and keeps the player appropriately framed during gameplay (walking, jumping).
    - (If Confiner is used) Camera is restricted within the defined `PolygonCollider2D` boundaries.
    - Main Camera has a `CinemachineBrain`.
- **Manual Approach (if chosen):**
    - `CameraController.cs` script is created and attached to the Main Camera.
    - Camera smoothly follows the `target` (Player).
    - `offset` and `smoothSpeed` are configurable.
    - (If bounds are used) Camera is clamped within `minBounds` and `maxBounds`.
- Camera operates correctly in a test scene with player movement.

# Test Strategy:
- **Cinemachine:**
    - Play the game in a test scene.
    - Observe camera behavior as the player moves, jumps, and falls.
    - Adjust `Framing Transposer` settings (damping, lookahead, dead/soft zones) until the feel is right.
    - If using `CinemachineConfiner`, test camera behavior at the edges of the defined boundaries.
- **Manual:**
    - Play the game in a test scene.
    - Verify smooth camera follow.
    - Adjust `smoothSpeed` and `offset` for desired feel.
    - If using bounds, test that camera stops at `minBounds` and `maxBounds`.
    - Check that `smoothSpeed` being multiplied by `Time.deltaTime` in Lerp makes it framerate independent (or appears to). A common alternative is `desiredPos = Vector3.Lerp(currentPos, targetPos, smoothFactor);` where `smoothFactor` is small (e.g., 0.05) and NOT multiplied by deltaTime, which gives an exponential decay type of smoothing often used. The plan's example of `smoothSpeed` implies rate, so *Time.deltaTime makes sense.

# Notes/Questions:
- The Implementation Plan **recommends Cinemachine**. This should be the primary approach. The manual implementation is a fallback.
- For Cinemachine `Framing Transposer`, extensive tweaking of parameters is usually needed to achieve the desired camera feel.
- Camera bounds (whether via Cinemachine Confiner or manual clamping) are important for preventing the camera from showing areas outside the playable level. These bounds might come from `LevelSettingsSO` (Task 1.3.3) in a more integrated system.
- If using manual implementation, the `smoothSpeed * Time.deltaTime` in `Vector3.Lerp` for position smoothing should be carefully considered. A fixed smoothing factor (e.g., `0.1f`) applied each frame often yields more consistent results across varying frame rates than multiplying by `Time.deltaTime` in this specific lerp usage pattern. Or, use `Vector3.SmoothDamp`. For now, following the more direct interpretation of "smoothSpeed".