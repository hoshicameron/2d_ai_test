# Task ID: 5.3.1
# Parent Task ID: 5.3
# Title: Implement VFXManager (Optional, for Pooling)
# Status: pending
# Dependencies: None (core Unity functionality)
# Priority: medium # Becomes high if many VFX are used
# Estimated Effort: M
# Assignee: Unassigned

# Description:
(Optional) Implement `VFXManager.cs`, a Singleton or service for managing instantiation and pooling of common particle effect prefabs. This helps optimize performance by reusing VFX GameObjects.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/VFX/VFXManager.cs`
2.  **Namespace:** `PetalsOfHope.VFX`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/VFX/VFXManager.cs
    namespace PetalsOfHope.VFX
    {
        using UnityEngine;
        using System.Collections.Generic;

        public class VFXManager : MonoBehaviour // Singleton
        {
            public static VFXManager Instance { get; private set; }

            // Simple pooling mechanism: Dictionary of queues, one queue per VFX prefab type
            private Dictionary<string, Queue<ParticleSystem>> _vfxPool = new Dictionary<string, Queue<ParticleSystem>>();
            private Dictionary<string, GameObject> _vfxPrefabs = new Dictionary<string, GameObject>(); // To store original prefabs

            // Optional: Pre-warm specific pools
            [System.Serializable]
            public struct VFXPoolConfig
            {
                public string vfxName; // Key to use for this VFX
                public GameObject vfxPrefab;
                public int initialPoolSize;
            }
            public List<VFXPoolConfig> initialPools;


            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                    InitializePools();
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            private void InitializePools()
            {
                foreach (var config in initialPools)
                {
                    if (config.vfxPrefab == null || string.IsNullOrEmpty(config.vfxName)) continue;
                    
                    RegisterVFXPrefab(config.vfxName, config.vfxPrefab); // Store prefab reference
                    
                    if (!_vfxPool.ContainsKey(config.vfxName))
                    {
                        _vfxPool[config.vfxName] = new Queue<ParticleSystem>();
                    }

                    for (int i = 0; i < config.initialPoolSize; i++)
                    {
                        GameObject vfxInstanceObj = Instantiate(config.vfxPrefab, transform); // Parent to manager
                        ParticleSystem ps = vfxInstanceObj.GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            vfxInstanceObj.SetActive(false);
                            _vfxPool[config.vfxName].Enqueue(ps);
                        }
                        else
                        {
                            Debug.LogWarning($"VFX Prefab '{config.vfxPrefab.name}' for '{config.vfxName}' does not have a ParticleSystem component.", config.vfxPrefab);
                            Destroy(vfxInstanceObj); // Cleanup
                        }
                    }
                }
            }
            
            // Call this if you want to register prefabs dynamically instead of via initialPools list
            public void RegisterVFXPrefab(string vfxName, GameObject prefab)
            {
                if (!_vfxPrefabs.ContainsKey(vfxName))
                {
                    _vfxPrefabs[vfxName] = prefab;
                }
            }


            /// <summary>
            /// Plays a VFX by its registered name from the pool at a given position and rotation.
            /// </summary>
            public ParticleSystem PlayEffect(string vfxName, Vector3 position, Quaternion rotation, Transform parent = null)
            {
                if (!_vfxPool.ContainsKey(vfxName) || !_vfxPrefabs.ContainsKey(vfxName))
                {
                    Debug.LogWarning($"VFX with name '{vfxName}' not registered or pooled.", this);
                    return null; 
                }

                ParticleSystem psToPlay = null;
                if (_vfxPool[vfxName].Count > 0)
                {
                    psToPlay = _vfxPool[vfxName].Dequeue();
                    psToPlay.gameObject.SetActive(true);
                }
                else // Pool empty, instantiate a new one (and optionally add to pool if allowed to grow)
                {
                    GameObject newVfxObj = Instantiate(_vfxPrefabs[vfxName]);
                    psToPlay = newVfxObj.GetComponent<ParticleSystem>();
                    if (psToPlay == null) {
                        Debug.LogError($"Instantiated VFX '{vfxName}' but it has no ParticleSystem component!");
                        Destroy(newVfxObj);
                        return null;
                    }
                    // Optionally add this new instance to the pool if dynamic growth is desired
                    // Or just let it be a one-off (less ideal for pooling benefits)
                }

                if (psToPlay != null)
                {
                    psToPlay.transform.position = position;
                    psToPlay.transform.rotation = rotation;
                    if (parent != null) psToPlay.transform.SetParent(parent, true); // World position stays
                    
                    psToPlay.Clear(); // Clear previous state if any
                    psToPlay.Play();

                    // Automatically return to pool after duration
                    StartCoroutine(ReturnToPoolAfterDuration(vfxName, psToPlay, psToPlay.main.duration + psToPlay.main.startLifetime.constantMax));
                }
                return psToPlay;
            }

            /// <summary>
            /// Plays a VFX from a direct prefab reference (less optimal if called often, better to register and use by name).
            /// </summary>
            public ParticleSystem PlayEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, Transform parent = null)
            {
                if (effectPrefab == null) return null;
                
                // For one-off plays without pooling by name, just instantiate
                GameObject vfxInstanceObj = Instantiate(effectPrefab, position, rotation);
                if (parent != null) vfxInstanceObj.transform.SetParent(parent, true);
                
                ParticleSystem ps = vfxInstanceObj.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    // Auto-destroy after duration for non-pooled direct prefabs
                    Destroy(vfxInstanceObj, ps.main.duration + ps.main.startLifetime.constantMax); 
                }
                else
                {
                    Debug.LogWarning("Provided effectPrefab does not have a ParticleSystem component.", effectPrefab);
                    Destroy(vfxInstanceObj); // Cleanup if no particle system
                }
                return ps;
            }


            private System.Collections.IEnumerator ReturnToPoolAfterDuration(string vfxName, ParticleSystem psInstance, float duration)
            {
                yield return new WaitForSeconds(duration);
                if (psInstance != null && psInstance.gameObject.activeSelf) // Check if still active (might have been manually returned/destroyed)
                {
                    psInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Stop and clear particles
                    psInstance.gameObject.SetActive(false);
                    if (psInstance.transform.parent != transform) psInstance.transform.SetParent(transform); // Re-parent to manager
                    
                    if (_vfxPool.ContainsKey(vfxName))
                    {
                        _vfxPool[vfxName].Enqueue(psInstance);
                    }
                    else
                    {
                        // Pool for this type was somehow lost, just destroy
                        Destroy(psInstance.gameObject);
                    }
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- (If implemented) `VFXManager.cs` (as a Singleton MonoBehaviour) is created.
- It can initialize pools for specified VFX prefabs with an initial size.
- `PlayEffect(string vfxName, Vector3 position, Quaternion rotation, Transform parent)` method:
    - Retrieves a `ParticleSystem` from the pool for the given `vfxName`.
    - Activates it, sets its position/rotation/parent, and plays it.
    - If pool is empty, instantiates a new one (optionally adds to pool or just uses it as a one-off).
- `PlayEffect(GameObject effectPrefab, ...)` method allows playing non-pooled prefabs (which auto-destroy).
- Played particle systems are automatically returned to the pool (deactivated) after their duration.
- Script compiles without errors.

# Test Strategy:
- Manual Testing:
    - Create `VFXManager` GameObject. Configure `initialPools` with a test particle effect prefab.
    - Create a test script that calls `VFXManager.Instance.PlayEffect("TestVFXName", position, rotation)`.
    - Verify VFX plays at the correct location.
    - Verify multiple calls reuse pooled instances (observe Hierarchy, check for new instantiations vs. reactivations).
    - Verify VFX is deactivated and returned to pool after its duration.
    - Test the direct prefab playback method too.

# Notes/Questions:
- This `VFXManager` is marked optional in the plan. If not implemented, VFX will be instantiated/destroyed directly by gameplay scripts (less performant for frequent effects).
- The pooling logic is basic. More advanced poolers might handle dynamic resizing, max limits, etc.
- Particle systems on prefabs should have "Play On Awake" unchecked, and "Stop Action" set to "Disable" or "Callback" if the pool handles deactivation. For this auto-return coroutine, "Disable" is fine, but the coroutine explicitly deactivates.
- `ps.main.duration + ps.main.startLifetime.constantMax` is a common way to estimate total particle system lifetime.