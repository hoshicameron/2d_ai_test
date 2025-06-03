# Task ID: 4.1.4
# Parent Task ID: 4.1
# Title: Implement Checkpoint System
# Status: pending
# Dependencies: 1.2, 1.4.5 # Event Bus, SaveLoadManager (for saving checkpoint data)
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement a checkpoint system. This includes a `Checkpoint.cs` script that raises an event when the player reaches it. A manager system (e.g., `GameManager` or a new `PlayerRespawnSystem`) will listen to this event and store the active checkpoint's position for respawning.

# Details:
1.  **Create `PlayerReachedCheckpointEventSO`:**
    *   Type: `TypedEventSO<Vector3>` (payload: checkpoint position) or `TypedEventSO<Transform>` (payload: checkpoint transform). Let's use `Vector3` for simplicity of saving spawn position.
    *   Location: `Assets/_Project/ScriptableObjects/Events/Gameplay/PlayerReachedCheckpointEventSO.asset`
    *   Name: `PlayerReachedCheckpointEventSO`.

2.  **Create `Checkpoint.cs` Script:**
    *   File Location: `Assets/_Project/Scripts/Gameplay/LevelElements/Checkpoint.cs`
    *   Namespace: `PetalsOfHope.Gameplay.LevelElements`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Gameplay/LevelElements/Checkpoint.cs
        namespace PetalsOfHope.Gameplay.LevelElements
        {
            using UnityEngine;
            using PetalsOfHope.Core.Events; // For TypedEventSO

            public class Checkpoint : MonoBehaviour
            {
                [Tooltip("Event raised when player activates this checkpoint. Payload is checkpoint's position.")]
                [SerializeField] private Vector3EventSO _playerReachedCheckpointEventSO; // Assuming Vector3EventSO exists

                [Tooltip("Visual to indicate checkpoint is active (optional).")]
                [SerializeField] private GameObject _activeVisual;
                [Tooltip("Visual to indicate checkpoint is inactive (optional).")]
                [SerializeField] private GameObject _inactiveVisual;

                private bool _isActivated = false;
                private static Checkpoint _currentActiveCheckpoint = null; // Static to track across all checkpoints

                private void Awake()
                {
                    SetVisuals(false); // Start as inactive appearance
                }

                private void OnTriggerEnter2D(Collider2D collision)
                {
                    if (!_isActivated && collision.CompareTag("Player"))
                    {
                        ActivateCheckpoint();
                    }
                }

                private void ActivateCheckpoint()
                {
                    if (_currentActiveCheckpoint != null && _currentActiveCheckpoint != this)
                    {
                        _currentActiveCheckpoint.DeactivateVisuals(); // Deactivate visuals of previous checkpoint
                    }

                    _isActivated = true;
                    _currentActiveCheckpoint = this;
                    SetVisuals(true);
                    
                    _playerReachedCheckpointEventSO?.Raise(transform.position); // Raise event with this checkpoint's position
                    // Debug.Log($"Checkpoint activated at: {transform.position}");
                }

                private void SetVisuals(bool isActive)
                {
                    if (_activeVisual != null) _activeVisual.SetActive(isActive);
                    if (_inactiveVisual != null) _inactiveVisual.SetActive(!isActive);
                }
                
                public void DeactivateVisuals() // Called by newly activated checkpoint
                {
                    _isActivated = false; // Allow re-activation if player passes again, or keep it permanently active visually once touched
                    SetVisuals(false);
                }

                // Optional: method to force reset all checkpoints (e.g. on level start if not saving active checkpoint)
                public static void ResetAllCheckpoints()
                {
                    _currentActiveCheckpoint = null; 
                    // Find all Checkpoint objects and call a reset method on them if needed for visuals.
                }
            }
        }
        ```

3.  **Create `PlayerRespawnSystem.cs` (or integrate into `GameManager`):**
    *   File Location: `Assets/_Project/Scripts/Systems/PlayerRespawnSystem.cs`
    *   Namespace: `PetalsOfHope.Systems`
    *   This system listens to `PlayerReachedCheckpointEventSO` and `PlayerDiedEventSO`.
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Systems/PlayerRespawnSystem.cs
        namespace PetalsOfHope.Systems
        {
            using UnityEngine;
            using PetalsOfHope.Core.Events;
            using UnityEngine.SceneManagement; // For reloading scene if needed

            public class PlayerRespawnSystem : MonoBehaviour // Could be a Singleton
            {
                [Header("Event Listeners")]
                [SerializeField] private Vector3EventSO _playerReachedCheckpointEventSO; // Listen
                [SerializeField] private GameEventSO _playerDiedEventSO;              // Listen

                [Header("Event Raisers")]
                [SerializeField] private GameEventSO _onPlayerRespawnEventSO;          // Raise after respawn

                private Vector3 _lastCheckpointPosition;
                private bool _hasCheckpoint = false;
                private GameObject _playerInstance; // Cached player instance

                // public static PlayerRespawnSystem Instance { get; private set; } // For Singleton

                private void Awake()
                {
                    // Singleton setup if used
                    // if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
                    // else { Destroy(gameObject); return; }
                    
                    // Find player at start, or Player can register itself.
                    // For simplicity, assume player is tagged "Player" and present at start.
                    _playerInstance = GameObject.FindGameObjectWithTag("Player");
                    if (_playerInstance != null && !_hasCheckpoint) { // Set initial spawn point if no checkpoint saved
                        _lastCheckpointPosition = _playerInstance.transform.position;
                    }
                }

                private void OnEnable()
                {
                    _playerReachedCheckpointEventSO?.RegisterListener(OnPlayerReachedCheckpoint);
                    _playerDiedEventSO?.RegisterListener(OnPlayerDied);
                }

                private void OnDisable()
                {
                    _playerReachedCheckpointEventSO?.UnregisterListener(OnPlayerReachedCheckpoint);
                    _playerDiedEventSO?.UnregisterListener(OnPlayerDied);
                }

                private void OnPlayerReachedCheckpoint(Vector3 checkpointPosition)
                {
                    _lastCheckpointPosition = checkpointPosition;
                    _hasCheckpoint = true;
                    // Debug.Log($"PlayerRespawnSystem: New checkpoint saved at {_lastCheckpointPosition}");
                    // Future: Save this _lastCheckpointPosition to SaveLoadManager
                }

                private void OnPlayerDied()
                {
                    // Debug.Log("PlayerRespawnSystem: Player died. Respawning...");
                    // Consider a delay before respawn (e.g., using a coroutine)
                    RespawnPlayer();
                }

                private void RespawnPlayer()
                {
                    if (_playerInstance == null)
                    {
                        _playerInstance = GameObject.FindGameObjectWithTag("Player");
                        if (_playerInstance == null) {
                            Debug.LogError("Player instance not found. Cannot respawn.");
                            return;
                        }
                    }

                    // Option 1: Reset player state and move to checkpoint
                    _playerInstance.transform.position = _lastCheckpointPosition;
                    // Re-enable PlayerController, reset health, etc.
                    var playerController = _playerInstance.GetComponent<PetalsOfHope.Gameplay.Player.PlayerController>();
                    if (playerController != null) playerController.enabled = true;
                    
                    var playerHealth = _playerInstance.GetComponent<PetalsOfHope.Gameplay.Player.PlayerHealth>();
                    if (playerHealth != null) {
                        // playerHealth.ResetHealth(); // Add ResetHealth() method to PlayerHealth
                        // For now, we can call Start() again on PlayerHealth by disabling/enabling or a dedicated method
                        playerHealth.enabled = false; playerHealth.enabled = true; // Hacky way to re-init health
                    }
                    var rb = _playerInstance.GetComponent<Rigidbody2D>();
                    if(rb != null) {
                        rb.velocity = Vector2.zero;
                        rb.isKinematic = false;
                    }

                    _onPlayerRespawnEventSO?.Raise();
                    // Debug.Log($"Player respawned at {_lastCheckpointPosition}");

                    // Option 2: Reload current scene (simpler for resetting everything but loses temp state)
                    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                
                // Methods for Save/Load system integration
                public object CaptureState() {
                    return new SaveData { checkpointPosition = _lastCheckpointPosition, hasCheckpoint = _hasCheckpoint };
                }
                public void RestoreState(object state) {
                    if (state is SaveData saveData) {
                        _lastCheckpointPosition = saveData.checkpointPosition;
                        _hasCheckpoint = saveData.hasCheckpoint;
                        if (_hasCheckpoint && _playerInstance != null) { // On game load, move player to checkpoint
                             _playerInstance.transform.position = _lastCheckpointPosition;
                        }
                    }
                }
                [System.Serializable]
                private struct SaveData { public Vector3 checkpointPosition; public bool hasCheckpoint; }
            }
        }
        ```
    *   Ensure this system (or `GameManager`) is present in scenes.
    *   It needs to be `ISaveable` if checkpoint location persists across game sessions (see Task 3.4.5).

4.  **Create Checkpoint Prefab:**
    *   Location: `Assets/_Project/Prefabs/LevelElements/Interactables/CheckpointPrefab.prefab`
    *   GameObject with a `SpriteRenderer` (e.g., a flag or glowing orb).
    *   Add `BoxCollider2D`, set `Is Trigger` to true.
    *   Attach `Checkpoint.cs` script.
    *   Assign `PlayerReachedCheckpointEventSO`.
    *   Configure `_activeVisual` and `_inactiveVisual` GameObjects as children of the prefab if visual state change is desired.

# Acceptance Criteria:
- `PlayerReachedCheckpointEventSO` (Vector3EventSO) is created.
- `Checkpoint.cs` script raises this event with its position when player enters its trigger.
- `PlayerRespawnSystem` (or similar manager) listens to checkpoint and player death events.
- On player death, the player is repositioned at the last activated checkpoint's position.
- Player state (health, controls) is reset upon respawn.
- Checkpoint prefab is created and functional, optionally with visual feedback for activation.
- (Later) Active checkpoint position is saved and loaded (part of Task 3.4.5).

# Test Strategy:
- Manual Testing:
    - Place checkpoint prefabs in a test level.
    - Place a `PlayerRespawnSystem` in the scene.
    - Player touches a checkpoint: verify event is raised (log), `PlayerRespawnSystem` updates its stored position.
    - Player dies (e.g., via hazard): verify player respawns at the last checkpoint.
    - Test multiple checkpoints: activate one, then another, then die. Player should respawn at the latest one.
    - Verify player health and controls are restored after respawn.

# Notes/Questions:
- The `PlayerRespawnSystem` needs a way to reset the player's state (health, re-enable `PlayerController`, etc.). This might involve adding a `ResetState()` method to `PlayerController` and `PlayerHealth`. The current example uses a simple disable/enable hack for `PlayerHealth` re-initialization.
- The `PlayerRespawnSystem` itself should be an `ISaveable` (Task 1.4.1) if the last checkpoint needs to persist across game sessions (Task 3.4.5 focuses on general progression saving). Added basic `CaptureState/RestoreState` to `PlayerRespawnSystem`.
- A `Vector3EventSO` needs to be defined similar to `IntEventSO` (Task 1.2.3) if not already present.
- Static `_currentActiveCheckpoint` in `Checkpoint.cs` is a simple way to manage visuals for only one active checkpoint.