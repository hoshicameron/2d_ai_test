# **Petals of Hope - Implementation Plan**

## **1. Introduction**

This document outlines the implementation plan for *Petals of Hope*, a 2D platformer game developed in Unity. Based on the architectural design document and detailed phase breakdowns, this plan provides a structured approach to implementing the game's systems, establishing dependencies, and setting milestones for development. The plan is organized into phases, with each phase building upon the previous one to ensure a logical progression of development.

## **2. Development Phases Overview**

### **2.1. Phase 1: Foundation Systems**
- Project Setup
- Event Bus System
- ScriptableObject Data Management
- Save/Load System

### **2.2. Phase 2: Core Gameplay Systems**
- Input System
- Reusable Animation Controller
- State Machine System
- Player Controller & States
- Player Health
- Camera System

### **2.3. Phase 3: Enemy Systems**
- EnemyBase Class & IDamageable Interface
- AI System (State Machine based)
- Basic Enemy Types & Prefabs

### **2.4. Phase 4: Level Design & Game Progression**
- Level Design Tools & Workflow (Tilemaps, Prefabs)
- Scene Management System
- Talisman Award System (Programmatic, UI Notification)
- Game Progression System (Level/Ability Unlocking)

### **2.5. Phase 5: Polish & Refinement**
- UI Systems (UI Manager, HUD, Menus)
- Audio Systems (Audio Manager, Audio Events)
- Visual Effects (VFX Manager, Particle Effects)
- Performance Optimization & Profiling

## **3. Detailed Implementation Plan**

### **3.1. Phase 1: Foundation Systems (Weeks 1-2)**

#### **3.1.1. Project Setup**
- Create Unity project using **2D URP Core** template (Unity 2022.x or newer).
- Initialize Git repository, configure `.gitignore` for Unity, set up remote.
- Establish project folder structure as detailed in `Phase1_Foundation_Systems.md` (e.g., `Assets/_Project/Scripts/{Core, Gameplay, Enemies, Systems, UI, Editor}`, `_Project/{Prefabs, ScriptableObjects, Art, Audio, Scenes, Settings}`).
- Import essential Unity packages: Input System, Cinemachine (if not included by template).
- Establish root namespace (e.g., `PetalsOfHope`).

#### **3.1.2. Event Bus System**
- **Namespace:** `PetalsOfHope.Core.Events`
- Implement `BaseEventSO.cs` (abstract `ScriptableObject`) in `_Project/Scripts/Core/Events/Base/`.
- Implement `GameEventSO.cs` (parameterless, `Action OnEventRaised`) in `_Project/Scripts/Core/Events/`.
- Implement `TypedEventSO<T>.cs` (generic, `Action<T> OnEventRaised`) in `_Project/Scripts/Core/Events/`.
- Implement `EventListener.cs` (MonoBehaviour, listens to `GameEventSO`, invokes `UnityEvent`) in `_Project/Scripts/Core/Events/Listeners/`.
- Implement `TypedEventListener<T>.cs` (MonoBehaviour, listens to `TypedEventSO<T>`, invokes `UnityEvent<T>`).
- Create editor tools for event debugging (e.g., "Raise" button in Inspector).
- Unit test event propagation (listener registration, unregistration, raising events).
- Create initial `EventSO` assets in `_Project/ScriptableObjects/Events/`.

#### **3.1.3. ScriptableObject Data Management**
- **Namespace:** `PetalsOfHope.Core.Data`
- Implement base `EntityStatsSO.cs` (abstract `ScriptableObject`, common stats like `maxHealth`) in `_Project/Scripts/Core/Data/Stats/`.
- Create derived classes:
    - `PlayerStatsSO.cs` (player-specific stats: `movementSpeed`, `jumpForce`) in `_Project/Scripts/Data/Player/`.
    - `EnemyStatsSO.cs` (enemy stats: `patrolSpeed`, `chaseSpeed`, `detectionRange`, `damage`) in `_Project/Scripts/Data/Enemies/`.
    - `BossStatsSO.cs` (inherits `EnemyStatsSO` or `EntityStatsSO`) in `_Project/Scripts/Data/Enemies/Bosses/`.
- Implement `AbilitySO.cs` (abstract `ScriptableObject`, common properties: `abilityName`, `icon`, `cooldown`) in `_Project/Scripts/Data/Abilities/`.
- Create placeholder derived ability classes (e.g., `DoubleJumpSO`, `DashSO`) in `_Project/Scripts/Data/Abilities/Types/`.
- Implement `LevelSettingsSO.cs` (`gravityScale`, `backgroundMusic`) in `_Project/Scripts/Data/Levels/`.
- Create editor tools for data management/validation (optional).
- Create initial data assets (e.g., `DefaultPlayerStats`, `WolfEnemyStats`) in `_Project/ScriptableObjects/` subfolders.

#### **3.1.4. Save/Load System**
- **Namespace:** `PetalsOfHope.Core.Persistence`
- **Interfaces** (in `_Project/Scripts/Core/Persistence/Interfaces/`):
    - `IDataService.cs`: Defines `Save<T>`, `Load<T>`, `Delete`, `DeleteAll`, `HasKey`.
    - `ISaveable.cs`: Defines `UniqueID { get; }`, `object CaptureState()`, `void RestoreState(object state)`.
- **Event Bus Integration:** Define `GameEventSO` assets for `OnBeforeSave`, `OnAfterLoad`, `OnSaveFailed`, `OnLoadFailed`.
- **Concrete Data Services** (in `_Project/Scripts/Core/Persistence/Services/`):
    - `PlayerPrefsDataService.cs`: Implements `IDataService` using `PlayerPrefs` (with JSON serialization).
    - `JsonDataService.cs`: Implements `IDataService` using file-based JSON storage (`Application.persistentDataPath`).
- **`SaveLoadManager.cs`** (MonoBehaviour, in `_Project/Scripts/Core/Persistence/`):
    - Holds active `IDataService`.
    *   Finds all `ISaveable` components.
    - Manages `SaveGame()`, `LoadGame()`, `DeleteSave()` operations.
    - Aggregates state into a dictionary for saving.
    - Raises save/load related events.

### **3.2. Phase 2: Core Gameplay Systems (Weeks 3-5)**

#### **3.2.1. Input System**
- **Namespace:** `PetalsOfHope.Core.Input`
- Create `PlayerInputActions.inputactions` asset in `_Project/Settings/Input/`.
    - Define Action Maps: `Gameplay`, `UI`.
    - Define Actions: `Move` (Vector2), `Jump` (Button), `Dash` (Button), `Interact` (Button) for Gameplay; `Navigate` (Vector2), `Submit` (Button), `Cancel` (Button) for UI.
    - Generate C# class for the asset.
- Implement `InputReader.cs` (`ScriptableObject`, implements generated input interfaces) in `_Project/Scripts/Core/Input/`.
    - Fields for `TypedEventSO<Vector2>` (Move, Navigate) and `GameEventSO` (Jump, JumpCancelled, Dash, Interact, Submit, Cancel).
    - Initializes `PlayerInputActions`, sets callbacks, raises events on input.
    - Methods to switch action maps (`EnableGameplayInput()`, `EnableUIInput()`).
- Create `InputReader` SO asset in `_Project/ScriptableObjects/Input/` and link event assets.
- Test input with keyboard and gamepad.

#### **3.2.2. Reusable Animation Controller**
- **Namespace:** `PetalsOfHope.Core.Animation`
- Implement `AnimationController.cs` (MonoBehaviour) in `_Project/Scripts/Core/Animation/`.
    - Requires `Animator` component.
    - Caches `Animator` reference.
    - Public methods: `Play(string/int stateNameOrHash)`, `SetBool(string/int param, bool val)`, `SetFloat(string/int param, float val)`, `SetInteger(string/int param, int val)`, `SetTrigger(string/int param)`.
    - Decouples animation calls from state machines/controllers.

#### **3.2.3. State Machine System**
- **Namespace:** `PetalsOfHope.Core.StateMachine`
- Implement `BaseState.cs` (abstract class) in `_Project/Scripts/Core/StateMachine/`.
    - `protected StateMachine stateMachine;`
    - Abstract methods: `Enter()`, `Exit()`, `Update()`, `FixedUpdate()`.
- Implement `StateMachine.cs` (MonoBehaviour) in `_Project/Scripts/Core/StateMachine/`.
    - `private BaseState currentState;`
    - Methods: `Update()`, `FixedUpdate()` (call current state's methods), `ChangeState(BaseState newState)`, `Initialize(BaseState startingState)`.
- (Optional) Implement `Transition.cs` base class and `InputTransition.cs` example.
- Unit test state machine logic (initialization, transitions, updates).

#### **3.2.4. Player Controller & States**
- **Namespace:** `PetalsOfHope.Gameplay.Player`
- Implement `PlayerController.cs` (MonoBehaviour) in `_Project/Scripts/Gameplay/Player/`.
    - Requires `Rigidbody2D`, `Collider2D`, `StateMachine`, `AnimationController`.
    - Fields: `PlayerStatsSO`, `InputReader`, component references (`_rigidbody`, `_animationController`), state instances.
    - Movement params (`_moveInput`, `_jumpInput`), ground check (`groundCheckPoint`, `groundCheckRadius`, `groundLayer`, `IsGrounded`).
    - `Awake()`: Get components, instantiate states.
    - `Start()`: Initialize state machine with `IdleState`.
    - `OnEnable()`/`OnDisable()`: Register/unregister input event listeners.
    *   `Update()`: `CheckIfGrounded()`.
    - Input handlers: `HandleMoveInput(Vector2)`, `HandleJumpInput()`, `HandleJumpCancelledInput()`.
    - Public properties/methods for states to access (e.g., `Rigidbody`, `Stats`, `AnimationController`).
- Implement Player States (inheriting `BaseState`) in `_Project/Scripts/Gameplay/Player/States/`:
    - `IdleState.cs`: Transition on move input.
    - `MovingState.cs`: Apply horizontal movement based on `playerStats.MovementSpeed`. Transition on jump or no input. Control walk/run animation via `AnimationController`.
    - `JumpingState.cs`: Apply upward force (`playerStats.JumpForce`). Transition to `FallingState`. Control jump animation.
    - `FallingState.cs`: Manage gravity. Transition on ground. Control fall animation.
    - States use `player.AnimationController` to trigger animations.
- Raise `EventSO`s for significant player actions (e.g., `PlayerLandedEventSO`, `PlayerTookDamageEventSO`).

#### **3.2.5. Player Health**
- Implement `PlayerHealth.cs` (MonoBehaviour) in `_Project/Scripts/Gameplay/Player/`.
    - Fields: `PlayerStatsSO`, `playerDiedEvent` (GameEventSO), `playerHealthChangedEvent` (TypedEventSO<int>), `_currentHealth`.
    - `Start()`: Initialize health, raise `playerHealthChangedEvent`.
    - `TakeDamage(int amount)`: Reduce health, raise event, call `Die()` if health <= 0.
    - `Die()`: Raise `playerDiedEvent`, trigger death animation via `AnimationController`.

#### **3.2.6. Camera System**
- **Namespace:** `PetalsOfHope.Gameplay.Camera`
- **Using Cinemachine (Recommended):**
    - Add `Cinemachine Virtual Camera`, set `Follow` to Player.
    - Configure `Body` (e.g., `Framing Transposer`) for smooth follow.
    - Add `Cinemachine Confiner` with a `PolygonCollider2D` for boundaries if needed.
- **Manual Implementation (Alternative):**
    - `CameraController.cs` (MonoBehaviour).
    - Fields: `target` (Player), `smoothSpeed`, `offset`, `minBounds`, `maxBounds`.
    - `LateUpdate()`: Smoothly interpolate camera position towards target, clamp to bounds.

### **3.3. Phase 3: Enemy Systems (Weeks 6-8)**

#### **3.3.1. EnemyBase Class & IDamageable Interface**
- **Namespace:** `PetalsOfHope.Enemies.Core` and `PetalsOfHope.Interfaces`
- Implement `IDamageable.cs` interface: `void TakeDamage(int amount)`.
- Implement `EnemyBase.cs` (abstract MonoBehaviour) in `_Project/Scripts/Enemies/Core/`.
    - Implements `IDamageable`.
    - Requires `Rigidbody2D`, `Collider2D`, `AnimationController`.
    - Properties: `maxHealth`, `currentHealth`, `movementSpeed`, `damageAmount`.
    - Cached components: `_rigidbody`, `_collider`, `_animationController`.
    - `Awake()`: Cache components, initialize health.
    - `TakeDamage(int amount)`: Reduce health, trigger feedback/hurt animation, call `Die()`.
    - `Die()`: Trigger death animation/VFX, raise `EnemyDiedEventSO`, disable/destroy object.

#### **3.3.2. AI System (State Machine based)**
- **Namespace:** `PetalsOfHope.AI`
- **Core** (in `_Project/Scripts/AI/Core/`):
    - `State.cs` (abstract `ScriptableObject` or class): `EnterState(StateMachine owner)`, `ExecuteState(StateMachine owner)`, `ExitState(StateMachine owner)`.
    - `StateMachine.cs` (MonoBehaviour, for AI):
        - Requires `EnemyBase`, `AnimationController`.
        - References `EnemyBase Enemy`, `AnimationController`.
        - `[SerializeField] private State initialState;`, `private State currentState;`.
        - `Start()`: `ChangeState(initialState)`.
        - `Update()`: `currentState?.ExecuteState(this)`.
        - `ChangeState(State newState)`.
- **Concrete States** (ScriptableObjects in `_Project/Scripts/AI/States/`):
    - `PatrolState.cs`: Move between waypoints, detect player (`Physics2D.OverlapCircle`, `playerLayer`), transition to `ChaseState`. Use `owner.AnimationController` for walk/idle.
    *   `ChaseState.cs`: Move towards player, transition to `AttackState` if in range, or `PatrolState` if player lost. Use `owner.AnimationController` for run/chase.
    *   `AttackState.cs`: Perform attack logic, trigger attack animation via `owner.AnimationController`, deal damage, manage cooldown, transition to `ChaseState`.
    - `HurtState.cs`, `DeathState.cs` (can be triggered by `EnemyBase` or directly by damage events).
- **AI Configuration:** Create SO assets for each state type, configure parameters.
- States should raise events via Event Bus (e.g., `EnemyDetectedPlayerEventSO`).

#### **3.3.3. Enemy Types & Prefabs**
- **Namespace:** `PetalsOfHope.Enemies.Types`
- Create concrete enemy scripts (e.g., `WolfEnemy.cs`, `SpiderEnemy.cs`, `ArcherElfEnemy.cs`) inheriting from `EnemyBase` in `_Project/Scripts/Enemies/Types/`.
    - Customize specific behaviors, animations, or `StateMachine` configurations.
    - **Wolf:** Patrol and charge behavior.
    - **Spider:** Ceiling hanging, drop and climb.
    - **Archer Elf:** Projectile system, line of sight detection.
- Create prefabs for each enemy type in `_Project/Prefabs/Enemies/`.
    - Attach `EnemyBase` (or derived script), `Rigidbody2D`, `Collider2D`, `AnimationController`, `StateMachine` (AI).
    - Configure stats, initial AI state, and assign Animator Controller.
- Test enemy behaviors, state transitions, and interactions with the player.
- Implement a basic Projectile system if needed for Archer Elf.

### **3.4. Phase 4: Level Design & Game Progression (Weeks 9-11)**

#### **3.4.1. Level Design Tools & Workflow**
- **Namespace:** `PetalsOfHope.Editor.LevelDesign` (for tools)
- Utilize Unity's **Tilemap system**: Create Tile Palettes (`Window > 2D > Tile Palette`) with Rule Tiles.
- Create **Prefab Palettes**: Organize reusable level elements (platforms, hazards, interactive objects, enemy spawn points) as Prefabs.
- Implement hazard systems (spikes, pits) as prefabs or tile logic.
- Implement checkpoint system: `Checkpoint.cs` raises `PlayerReachedCheckpointEventSO`. `GameManager` or `PlayerRespawnSystem` listens.

#### **3.4.2. Scene Management System**
- **Namespace:** `PetalsOfHope.Systems.SceneManagement`
- Implement `SceneLoader.cs` service (using `UnityEngine.SceneManagement` or Addressables).
    - Methods: `LoadScene(string sceneName/int buildIndex)`, `LoadMainMenu()`, etc.
    - Implement scene transitions (e.g., fade-to-black).
- Create `SceneDataSO.cs` (`ScriptableObject`) for scene metadata (build index/path, display name).
- Implement level transition system (e.g., physical triggers in scene or UI selection).

#### **3.4.3. Talisman Award System**
- **Namespace:** `PetalsOfHope.Systems.Inventory` (or similar)
- `TalismanDataSO.cs` (`ScriptableObject` in `_Project/Data/Collectibles/Talismans/`): `talismanID`, `displayName`, `description`, `icon`.
- **Award Mechanism:** Talismans are awarded programmatically (e.g., after boss defeat via `BossDefeatedEventSO`).
- `InventorySystem.cs` (in `_Project/Scripts/Systems/Inventory/`):
    - Manages collected `TalismanDataSO`s.
    - `AddTalisman(TalismanDataSO talismanData)`.
    - Listens to award trigger events or is called directly.
    - Raises `TalismanAwardedEventSO(TalismanDataSO talisman)` on successful addition.
- **UI Notification:** `UIManager` listens to `TalismanAwardedEventSO` to display a notification.

#### **3.4.4. Player Ability System & Advanced States**
- **Namespace:** `PetalsOfHope.Gameplay.Player.Abilities` and `PetalsOfHope.Gameplay.Player.States`
- Implement `PlayerAbilities.cs` component on Player.
    - Manages available abilities, potentially checking against `InventorySystem` or `GameProgressionManager`.
    - Provides checks like `CanDoubleJump()`, `CanDash()`.
- Implement advanced player states (inheriting `BaseState`), triggered by input if ability is available:
    - `DoubleJumpState.cs`
    - `WallGrabState.cs`
    - `WallJumpState.cs`
    - `DashState.cs`
- Link these states to the `PlayerController`'s state machine.
- Abilities might be unlocked via Talismans (checked by `GameProgressionManager`).

#### **3.4.5. Game Progression System**
- **Namespace:** `PetalsOfHope.Systems.Progression`
- `GameProgressionManager.cs` (Singleton or service in `_Project/Scripts/Systems/Progression/`):
    - Manages overall progression (unlocked levels, abilities).
    - References `InventorySystem` to check collected talismans.
    - Methods: `IsLevelUnlocked(SceneDataSO sceneData)`, `IsAbilityUnlocked(AbilityID ability)`.
    - Listens to `TalismanAwardedEventSO` to update progression.
    - Raises `LevelUnlockedEventSO`, `AbilityUnlockedEventSO`.
- `LevelUnlockConditionSO.cs` (Optional `ScriptableObject` for complex unlock logic).
- Integrate with Level Selection UI, Scene Loader, `PlayerAbilities`.
- Ensure progression data (collected talismans, unlocked levels/abilities) is saved/loaded by implementing `ISaveable` on `InventorySystem` and `GameProgressionManager`.

### **3.5. Phase 5: Polish & Refinement (Weeks 12-14)**

#### **3.5.1. UI Systems**
- **Namespace:** `PetalsOfHope.UI`
- Decide on UI Technology: **UI Toolkit** (recommended for complex UIs) or **UGUI**.
- `UIManager.cs` (Singleton or service in `_Project/Scripts/UI/`):
    - Manages UI panels (Main Menu, Pause Menu, HUD, Options, Dialogue).
    - Handles showing/hiding/transitioning UI.
    - Listens to game events (`PlayerHealthChangedEventSO`, `TalismanAwardedEventSO`, `GameStateChangedEventSO`) to update UI.
- **UI Components/Controllers:**
    - `HUDController.cs`: Displays health, talisman count. Updates from events or `PlayerStats`/`InventorySystem`.
    - `MainMenuController.cs`, `PauseMenuController.cs`, `OptionsController.cs`: Handle button interactions, raise events, or call systems (`SceneLoader`, `AudioManager`).
- Implement narrative elements: Dialogue system UI, triggered by events or interactions.
- Ensure UI responsiveness across different resolutions.

#### **3.5.2. Audio Systems**
- **Namespace:** `PetalsOfHope.Audio`
- `AudioManager.cs` (Singleton or service in `_Project/Scripts/Audio/`):
    - Manages SFX and BGM playback using pooled Audio Sources.
    - Methods: `PlaySFX(AudioClip/AudioEventSO)`, `PlayBGM(AudioClip/AudioEventSO)`, volume controls.
    - Integrates with Unity's `AudioMixer` for groups (Master, SFX, BGM).
- `AudioEventSO.cs` (`ScriptableObject` in `_Project/Data/Audio/Events/`):
    - Defines `AudioClip[] clips` (variations), `volume`, `pitch`, `AudioMixerGroup`.
- Trigger sounds from gameplay components (`PlayerController`, `EnemyBase`, UI) by calling `AudioManager` or raising audio events.
- Implement BGM transitions between scenes/states.
- Add sound effects for player actions, enemies, UI interactions, environment.

#### **3.5.3. Visual Effects (VFX)**
- **Namespace:** `PetalsOfHope.VFX`
- Utilize Unity's Particle System (Shuriken), Shader Graph for custom effects.
- `VFXManager.cs` (Optional, for pooling, in `_Project/Scripts/VFX/`):
    - Manages instantiation and pooling of common particle effect prefabs.
    - `PlayEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation)`.
- Create effect prefabs (jump dust, landing impact, talisman collect, hit sparks, enemy death).
    - Configure "Play On Awake", "Stop Action" (Destroy or Disable for pooling).
- Trigger effects from gameplay components or via `VFXManager`.
- Implement screen effects (camera shake, flash) via `CameraManager` or dedicated system.
- Add animation polish: Ensure all character and enemy animations are smooth and impactful.

#### **3.5.4. Performance Optimization**
- **Profiling:** Regularly use Unity Profiler on target builds (CPU, GPU, Memory).
- **Object Pooling:** Implement for projectiles, particles, frequent SFX AudioSources, and potentially enemies.
- **CPU Optimization:**
    - Minimize `Update`/`FixedUpdate` work.
    - Cache component references (Rule III.3 from detailed phase docs).
    - Reduce GC Allocations: Avoid `new`, string ops, LINQ in hot paths.
- **GPU Optimization:**
    - Draw Calls: Sprite Atlases, Batching, GPU Instancing.
    - Overdraw: Optimize UI, opaque sprites.
    - Shaders: Simplify, use Shader LOD.
- **Memory Optimization:**
    - Track GC Allocations.
    - Texture Compression (ASTC, ETC2), reduce resolutions.
    - Audio Compression (Vorbis), stream BGM.
- Optimize event usage: Ensure no excessive event raising or listener overhead.
- Test on target platforms.

## **4. Dependencies and Critical Path**

### **4.1. System Dependencies (Simplified)**

Event Bus System → Input System → Player Controller & States
↓
ScriptableObject Data → Player & Enemy Stats → AI System
↓ ↓
Save/Load System EnemyBase & Types
↓
State Machine System → Player Abilities & Advanced States
↓
Scene Management & Level Design Tools
↓
Game Progression System
↓
UI, Audio, VFX Systems

(AnimationController is a utility used by Player/Enemy Controllers)

### **4.2. Critical Path**

1.  **Event Bus System & ScriptableObject Data Management** - Core data and communication.
2.  **Save/Load System (Basic Framework)** - Essential for progression.
3.  **State Machine System** - Foundation for player and enemy behaviors.
4.  **Input System** - Player interaction.
5.  **Player Controller & Basic States** (with AnimationController integration) - Core gameplay loop.
6.  **EnemyBase & Basic AI System** (with AnimationController integration) - Meaningful interaction.
7.  **Level Design Tools & Basic Scene Management** - Ability to create and test levels.
8.  **Game Progression & Talisman Award System** - Core game loop and objectives.

## **5. Testing Strategy**

### **5.1. Unit Testing**

- Implement unit tests for core systems:
    - Event Bus (propagation, listener management)
    - State Machine (transitions, state execution)
    - InputReader (event raising on input)
    - Save/Load DataServices (serialization/deserialization logic)
    - AI Decisions/State logic (if isolated)

### **5.2. Integration Testing**

- Test interactions between systems:
    - Player (Input → StateMachine → Movement/Abilities → Animation)
    - Player vs. Enemies (Combat, AI reactions, Damage)
    - Talisman Award → Inventory → Game Progression → UI/Ability Unlock
    - Save/Load with Player State, Inventory, and Game Progression.
    - Level Elements (Hazards, Checkpoints) with Player.

### **5.3. Playtest Schedule**

- **Alpha Testing (End of Week 8 - Milestone 3)**: Core gameplay mechanics, basic player-enemy interaction, first enemy types.
- **Beta Testing (End of Week 12 - after significant content from Phase 4)**: Near-complete game loop, multiple levels, talisman/ability progression.
- **Final Testing (Week 14 - Milestone 5)**: Polish, refinement, bug hunting, performance checks.

## **6. Risk Management**

### **6.1. Technical Risks**

| Risk                         | Probability | Impact | Mitigation                                                                          |
| ---------------------------- | ----------- | ------ | ----------------------------------------------------------------------------------- |
| State Machine complexity     | Medium      | High   | Early prototyping of complex states, thorough unit/integration testing, clear design. |
| Performance with many entities | Medium      | Medium | Implement object pooling early, LOD for AI/VFX, continuous profiling.               |
| Input system responsiveness  | Low         | High   | Use new Input System correctly, test across devices, prioritize input processing.     |
| Save/Load data corruption    | Medium      | High   | Robust (de)serialization, versioning for save data, thorough testing.               |

### **6.2. Schedule Risks**

| Risk                          | Probability | Impact | Mitigation                                                                              |
| ----------------------------- | ----------- | ------ | --------------------------------------------------------------------------------------- |
| Feature creep                 | High        | Medium | Strict adherence to MVP, prioritize core features, defer non-essentials.                |
| Art/Audio asset delays        | Medium      | Medium | Use placeholders, parallel development, clear communication with art/audio team.          |
| Complex ability interactions  | Medium      | High   | Incremental implementation, early prototyping of interactions, focused testing.         |
| Level design time             | Medium      | Medium | Efficient tools, clear design guidelines, reusable prefabs, early blocking of levels.     |

## **7. Tools and Resources**

### **7.1. Development Tools**

- Unity 2022.x or newer (with 2D URP, Cinemachine, Input System packages)
- Version Control: Git (with Git LFS for large assets if needed)
- Issue Tracking: GitHub/GitLab Issues, Trello, Jira, or similar.
- Documentation: Markdown files in repository, shared document platform.
- Communication: Slack, Discord, or similar.

### **7.2. Team Resources** (Assumed Roles)

- Programmers: Core systems, gameplay logic, AI, UI backend, tools.
- Artists: Character sprites/animations, environment tiles/props, UI assets, VFX.
- Designers: Level design, game balance, narrative, UI/UX flow, sound design direction.
- QA/Testers: Playtesting, bug reporting.

## **8. Milestones and Deliverables**

### **8.1. Milestone 1: Foundation (End of Week 2)**
- Event Bus, ScriptableObject Data, and Save/Load systems implemented.
- Project structure established and version controlled.
- Basic Unity project configured.

### **8.2. Milestone 2: Basic Gameplay (End of Week 5)**
- Input System, Reusable Animation Controller, and State Machine system implemented.
- Player Controller with basic states (Idle, Move, Jump, Fall) and health working.
- Camera system following player.
- Basic movement and physics feel good.

### **8.3. Milestone 3: Enemy Systems & Alpha (End of Week 8)**
- `EnemyBase` and AI `StateMachine` system implemented.
- At least 1-2 basic enemy types functioning with distinct behaviors (e.g., Wolf, Spider).
- Player-enemy combat interaction working (damage dealing/taking).
- **Alpha Testable Build:** Core gameplay mechanics demonstrable.

### **8.4. Milestone 4: Game Progression & Content (End of Week 11)**
- Level design tools and scene management functional.
- Talisman award system and player ability system (e.g., Double Jump, Dash) implemented.
- Game progression system managing unlocks.
- Initial set of levels (e.g., 2-3) designed and playable.
- Save/Load system fully integrated with progression.

### **8.5. Milestone 5: Polish & Complete Game (End of Week 14)**
- UI systems (HUD, Menus, basic Dialogue) implemented and functional.
- Audio systems (SFX, BGM) integrated.
- Visual effects added for key actions and ambiance.
- Performance profiled and optimized.
- **Beta Testable / Near-Final Build:** All core features complete, game playable from start to a defined endpoint.
- Final testing and bug fixing.

## **9. Conclusion**

This implementation plan provides a structured and detailed approach to developing *Petals of Hope*. By following this plan, the development team can work efficiently, with clear dependencies, milestones, and technical guidelines. The phased approach ensures that core systems are implemented first, providing a solid foundation for more complex features. Regular testing and risk management throughout development will help identify and address issues early, leading to a polished, performant, and engaging 2D platformer game that meets the vision outlined in the architectural design. This plan remains flexible to adapt to challenges and insights gained during development.