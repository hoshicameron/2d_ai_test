# Task ID: 2.2
# Parent Task ID: 2
# Title: Reusable Animation Controller Implementation
# Status: pending
# Dependencies: 1.1.2, 1.1.4 # Folder structure and namespace
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement a reusable `AnimationController.cs` MonoBehaviour that acts as a wrapper around Unity's `Animator` component. This component will provide a simplified interface for playing animations and setting parameters, decoupling animation logic from state machines or character controllers.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Animation/AnimationController.cs`
2.  **Namespace:** `PetalsOfHope.Core.Animation`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Animation/AnimationController.cs
    namespace PetalsOfHope.Core.Animation
    {
        using UnityEngine;

        [RequireComponent(typeof(Animator))]
        public class AnimationController : MonoBehaviour
        {
            private Animator _animator;

            // Caches for parameter hashes for performance
            private readonly Dictionary<string, int> _parameterHashMap = new Dictionary<string, int>();

            private void Awake()
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                {
                    Debug.LogError("AnimationController requires an Animator component on the same GameObject.", this);
                    enabled = false; // Disable this component if Animator is missing
                }
            }

            /// <summary>
            /// Plays an animation state.
            /// </summary>
            /// <param name="stateName">The name of the animation state to play.</param>
            public void Play(string stateName)
            {
                if (_animator == null) return;
                _animator.Play(stateName);
            }

            /// <summary>
            /// Plays an animation state using its hash for better performance.
            /// </summary>
            /// <param name="stateNameHash">The hash of the animation state name.</param>
            public void Play(int stateNameHash)
            {
                if (_animator == null) return;
                _animator.Play(stateNameHash);
            }

            /// <summary>
            /// Sets a boolean parameter on the Animator.
            /// </summary>
            /// <param name="paramName">The name of the boolean parameter.</param>
            /// <param name="value">The value to set.</param>
            public void SetBool(string paramName, bool value)
            {
                if (_animator == null) return;
                _animator.SetBool(GetParamHash(paramName), value);
            }

            /// <summary>
            /// Sets a boolean parameter on the Animator using its hash.
            /// </summary>
            /// <param name="paramHash">The hash of the boolean parameter name.</param>
            /// <param name="value">The value to set.</param>
            public void SetBool(int paramHash, bool value)
            {
                if (_animator == null) return;
                _animator.SetBool(paramHash, value);
            }

            /// <summary>
            /// Sets a float parameter on the Animator.
            /// </summary>
            /// <param name="paramName">The name of the float parameter.</param>
            /// <param name="value">The value to set.</param>
            public void SetFloat(string paramName, float value)
            {
                if (_animator == null) return;
                _animator.SetFloat(GetParamHash(paramName), value);
            }

            /// <summary>
            /// Sets a float parameter on the Animator using its hash.
            /// </summary>
            /// <param name="paramHash">The hash of the float parameter name.</param>
            /// <param name="value">The value to set.</param>
            public void SetFloat(int paramHash, float value)
            {
                if (_animator == null) return;
                _animator.SetFloat(paramHash, value);
            }

            /// <summary>
            /// Sets an integer parameter on the Animator.
            /// </summary>
            /// <param name="paramName">The name of the integer parameter.</param>
            /// <param name="value">The value to set.</param>
            public void SetInteger(string paramName, int value)
            {
                if (_animator == null) return;
                _animator.SetInteger(GetParamHash(paramName), value);
            }

            /// <summary>
            /// Sets an integer parameter on the Animator using its hash.
            /// </summary>
            /// <param name="paramHash">The hash of the integer parameter name.</param>
            /// <param name="value">The value to set.</param>
            public void SetInteger(int paramHash, int value)
            {
                if (_animator == null) return;
                _animator.SetInteger(paramHash, value);
            }

            /// <summary>
            /// Sets a trigger parameter on the Animator.
            /// </summary>
            /// <param name="paramName">The name of the trigger parameter.</param>
            public void SetTrigger(string paramName)
            {
                if (_animator == null) return;
                _animator.SetTrigger(GetParamHash(paramName));
            }

            /// <summary>
            /// Sets a trigger parameter on the Animator using its hash.
            /// </summary>
            /// <param name="paramHash">The hash of the trigger parameter name.</param>
            public void SetTrigger(int paramHash)
            {
                if (_animator == null) return;
                _animator.SetTrigger(paramHash);
            }

            /// <summary>
            /// Gets the hash for a parameter name, caching it for future use.
            /// </summary>
            private int GetParamHash(string paramName)
            {
                if (!_parameterHashMap.TryGetValue(paramName, out int hash))
                {
                    hash = Animator.StringToHash(paramName);
                    _parameterHashMap[paramName] = hash;
                }
                return hash;
            }

            // Optional: Method to get the underlying Animator component if direct access is needed
            public Animator GetAnimator()
            {
                return _animator;
            }
        }
    }
    ```

# Acceptance Criteria:
- `AnimationController.cs` MonoBehaviour is created in the specified location and namespace.
- It requires an `Animator` component on the same GameObject.
- It caches the `Animator` reference in `Awake()`.
- It provides public methods: `Play(string/int stateNameOrHash)`, `SetBool(string/int param, bool val)`, `SetFloat(string/int param, float val)`, `SetInteger(string/int param, int val)`, `SetTrigger(string/int param)`.
- Methods accepting string parameters for state/param names internally convert them to hashes using `Animator.StringToHash()` and cache them for performance.
- The script compiles without errors.
- If `Animator` is missing, an error is logged and the component disables itself.

# Test Strategy:
- Manual Verification & Integration Testing:
    - Create a test GameObject with a Unity `Animator` component and a simple Animator Controller (e.g., with states "Idle", "Run" and parameters "IsRunning" (bool), "Speed" (float)).
    - Attach the `AnimationController.cs` script to this GameObject.
    - Create a simple test script that gets a reference to `AnimationController` and calls its methods (e.g., `Play("Run")`, `SetBool("IsRunning", true)`).
    - In Play Mode, verify that the Animator component transitions to the correct states and parameter values are updated as expected.
    - Test both string-based and hash-based method overloads (manually pre-calculate a hash for testing).
    - Test behavior when Animator component is missing (verify error log and component disabled).

# Notes/Questions:
- This `AnimationController` serves as a facade, simplifying interaction with the `Animator` and promoting decoupling.
- Caching parameter name hashes (`Animator.StringToHash()`) is a good practice for performance, as string comparisons/lookups in `Animator` methods can be relatively slow if called frequently.
- The plan is clear about the required methods.
- This component will be used later by `PlayerController` and `EnemyBase`/AI `StateMachine`.