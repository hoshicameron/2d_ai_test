# **Petals of Hope - Architectural Design Document**

## **1. Introduction**

This document outlines the architectural design for *Petals of Hope*, a 2D platformer game developed in Unity. The architecture follows SOLID principles, DRY (Don't Repeat Yourself), and Clean Code practices to ensure maintainability, scalability, and ease of development. This document serves as a blueprint for implementing the game's systems in a modular, decoupled manner.

## **2. Core Architecture Overview**

### **2.1. Architectural Principles**

* **SOLID Principles**:
  * **Single Responsibility**: Each class has one responsibility and one reason to change.
  * **Open/Closed**: Systems are open for extension but closed for modification.
  * **Liskov Substitution**: Derived classes must be substitutable for their base classes.
  * **Interface Segregation**: Many specific interfaces are better than one general-purpose interface.
  * **Dependency Inversion**: Depend on abstractions, not concretions.

* **DRY (Don't Repeat Yourself)**: Avoid code duplication through proper abstraction.

* **Clean Code**: Prioritize readability, maintainability, and clear intent.

* **Component-Based Design**: Leverage Unity's component system for composition over inheritance.

### **2.2. High-Level Architecture Diagram**

```
+---------------------+     +----------------------+     +---------------------+
|                     |     |                      |     |                     |
|  Input System       |---->|  Core Game Systems   |<--->|  Event Bus System   |
|  (New Input System) |     |  (Player, Enemies)   |     |  (ScriptableObjects)|
|                     |     |                      |     |                     |
+---------------------+     +----------------------+     +---------------------+
          |                           |                           |
          v                           v                           v
+---------------------+     +----------------------+     +---------------------+
|                     |     |                      |     |                     |
|  State Machine      |<--->|  AI System           |<--->|  Data Management    |
|  (Player & Enemies) |     |  (Decision Making)   |     |  (ScriptableObjects)|
|                     |     |                      |     |                     |
+---------------------+     +----------------------+     +---------------------+
```

## **3. Event Bus System**

### **3.1. Overview**

The Event Bus system uses ScriptableObjects to facilitate communication between game components without direct references, promoting loose coupling and modular design.

### **3.2. Event Types**

* **Game Events**: Simple events with no parameters.
* **Typed Events**: Events with specific parameter types (int, float, Vector2, etc.).
* **Complex Events**: Events with custom data structures.

### **3.3. Implementation Structure**

* **BaseEventSO**: Abstract base class for all event ScriptableObjects.
* **GameEventSO**: Simple event with no parameters.
* **TypedEventSO<T>**: Generic event with a single parameter of type T.
* **EventListener**: MonoBehaviour that listens for events and responds with UnityEvents.

### **3.4. Event Flow**

1. **Publisher**: Any component can raise an event through a reference to an EventSO.
2. **Event**: The EventSO maintains a list of listeners and notifies them when raised.
3. **Listeners**: Components register with events they're interested in and respond accordingly.

### **3.5. Example Use Cases**

* **Player Health Change**: Health component raises a HealthChangedEvent, UI listens and updates display.
* **Enemy Defeated**: Enemy raises an EnemyDefeatedEvent, ScoreManager listens and updates score.
* **Ability Unlocked**: Talisman collection raises an AbilityUnlockedEvent, PlayerController listens and enables new abilities.

## **4. Data Management with ScriptableObjects**

### **4.1. Overview**

ScriptableObjects store game data separately from behavior, allowing for easy balancing, configuration, and reuse.

### **4.2. Data Categories**

* **Entity Data**: Stats for player, enemies, and bosses.
* **Level Data**: Environment-specific settings and parameters.
* **Ability Data**: Configuration for player abilities (Double Jump, Wall Jump, Dash).
* **Game Settings**: Global configuration settings.

### **4.3. Implementation Structure**

* **EntityStatsSO**: Base class for character statistics.
  * **PlayerStatsSO**: Player-specific stats (health, movement speed, jump height).
  * **EnemyStatsSO**: Enemy-specific stats (health, damage, detection range).
  * **BossStatsSO**: Boss-specific stats and attack patterns.

* **AbilitySO**: Configuration for player abilities.
  * **DoubleJumpSO**, **WallJumpSO**, **DashSO**: Specific ability configurations.

* **LevelSettingsSO**: Environment-specific settings (physics, hazards).

### **4.4. Benefits**

* **Designer-Friendly**: Non-programmers can tweak game balance without code changes.
* **Runtime Efficiency**: Data loaded once and referenced by multiple instances.
* **Persistence**: Data survives scene changes and play mode transitions.

## **5. Input System**

### **5.1. Overview**

The game uses Unity's new Input System for handling player input, providing flexibility, device support, and action mapping.

### **5.2. Input Actions**

* **Movement**: Left/Right movement (Vector2).
* **Jump**: Jump action (Button).
* **Special**: Special ability action (Button).
* **Menu**: Pause/Menu actions (Button).

### **5.3. Implementation Structure**

* **InputReader**: ScriptableObject that reads input and raises events.
  * Decouples input detection from input consumption.
  * Raises events through the Event Bus when input is detected.

* **Input Action Asset**: Defines all input mappings and actions.
  * Configured for keyboard, gamepad, and potentially touch controls.

### **5.4. Integration with State Machine**

* Input events trigger state transitions in the Player State Machine.
* Example: Jump button press raises JumpInputEvent, which triggers transition from Grounded to Jumping state.

## **6. State Machine System**

### **6.1. Overview**

A flexible state machine system manages both player and enemy behaviors, with different transition mechanisms for each.

### **6.2. Core Components**

* **State**: Defines behavior during a specific state.
* **StateMachine**: Manages states and transitions.
* **Transition**: Defines conditions for moving between states.

### **6.3. Implementation Structure**

* **BaseState**: Abstract class defining state interface.
  * **Enter()**: Called when entering the state.
  * **Exit()**: Called when exiting the state.
  * **Update()**: Called every frame while in the state.
  * **FixedUpdate()**: Called every physics update while in the state.

* **StateMachine**: Manages current state and transitions.
  * **ChangeState()**: Changes to a new state.
  * **RegisterState()**: Adds a state to the machine.
  * **RegisterTransition()**: Adds a transition between states.

* **Transition**: Base class for state transitions.
  * **InputTransition**: Transitions based on player input (for Player).
  * **DecisionTransition**: Transitions based on AI decisions (for Enemies).

### **6.4. Player States**

* **IdleState**: Player is stationary.
* **MovingState**: Player is walking horizontally.
* **JumpingState**: Player is in a jump.
* **FallingState**: Player is falling.
* **DoubleJumpState**: Player is performing a double jump.
* **WallGrabState**: Player is grabbing a wall.
* **WallJumpState**: Player is jumping off a wall.
* **DashState**: Player is performing a dash.
* **HurtState**: Player is taking damage.

### **6.5. Enemy States**

* **IdleState**: Enemy is inactive.
* **PatrolState**: Enemy is patrolling.
* **ChaseState**: Enemy is pursuing the player.
* **AttackState**: Enemy is attacking.
* **HurtState**: Enemy is taking damage.
* **DeathState**: Enemy is dying.

## **7. AI System**

### **7.1. Overview**

A modular AI system using decisions, actions, and transitions to control enemy behavior.

### **7.2. Core Components**

* **Decision**: Evaluates a condition and returns true/false.
* **Action**: Performs a behavior when executed.
* **AIController**: Manages decisions and actions for an enemy.

### **7.3. Implementation Structure**

* **BaseDecision**: Abstract class for all AI decisions.
  * **Decide()**: Evaluates a condition and returns true/false.
  * Examples: PlayerInRangeDecision, HealthBelowThresholdDecision, TimerElapsedDecision.

* **BaseAction**: Abstract class for all AI actions.
  * **Execute()**: Performs the action.
  * Examples: MoveToTargetAction, AttackAction, PatrolAction.

* **AIController**: Manages the AI's decision-making process.
  * Links to the StateMachine for state transitions.
  * Evaluates decisions and executes actions based on current state.

### **7.4. Enemy-Specific AI**

* **Wolf AI**:
  * Patrols back and forth.
  * Charges when player is detected within range.
  * Decisions: PlayerInDetectionRange, PatrolPointReached.
  * Actions: PatrolAction, ChargeAtPlayerAction.

* **Spider AI**:
  * Hangs from ceiling.
  * Drops when player passes underneath.
  * Climbs back up after a delay.
  * Decisions: PlayerBelowDecision, DropTimeElapsedDecision.
  * Actions: HangAction, DropAction, ClimbAction.

* **Archer Elf AI**:
  * Stands still.
  * Shoots when player is in line of sight.
  * Decisions: PlayerInLineOfSightDecision, CooldownElapsedDecision.
  * Actions: IdleAction, ShootArrowAction.

### **7.5. Integration with State Machine**

* AI decisions trigger state transitions in the Enemy State Machine.
* Example: PlayerInRangeDecision returns true, triggering transition from Patrol to Chase state.

## **8. Component Interaction**

### **8.1. Player Component Structure**

* **PlayerController**: Main component managing the player.
  * References PlayerStatsSO for configuration.
  * Contains StateMachine for state management.
  * Listens to input events from InputReader.

* **PlayerHealth**: Manages player health and damage.
  * Raises events when health changes or player dies.

* **PlayerAbilities**: Manages special abilities (Double Jump, Wall Jump, Dash).
  * Enabled/disabled based on collected talismans.
  * Configured via AbilitySO instances.

### **8.2. Enemy Component Structure**

* **EnemyController**: Main component managing an enemy.
  * References EnemyStatsSO for configuration.
  * Contains StateMachine for state management.
  * Contains AIController for decision making.

* **EnemyHealth**: Manages enemy health and damage.
  * Raises events when health changes or enemy dies.

* **EnemyAttack**: Manages enemy attack behavior.
  * Different implementations for different enemy types.

### **8.3. Communication Flow Example**

**Player Jumping on Enemy Head:**

1. Collision detected between player feet and enemy head.
2. EnemyHealth receives damage and raises HealthChangedEvent.
3. EnemyController listens to HealthChangedEvent and transitions to HurtState.
4. If health <= 0, transitions to DeathState and raises EnemyDefeatedEvent.
5. ScoreManager listens to EnemyDefeatedEvent and updates score.
6. DropManager listens to EnemyDefeatedEvent and potentially spawns a coin.

## **9. Performance Considerations**

### **9.1. Object Pooling**

* Implement object pooling for frequently spawned objects:
  * Projectiles (arrows, knives, snowballs)
  * Particles (death effects, hit effects)
  * Small enemies (slimes)

### **9.2. Event Optimization**

* Use appropriate event types based on frequency and complexity.
* Consider batching frequent events to reduce overhead.

### **9.3. State Machine Efficiency**

* Minimize state transitions during high-activity periods.
* Use lightweight state implementations for frequently changed states.

## **10. Extensibility**

### **10.1. Adding New Enemies**

1. Create new EnemyStatsSO with appropriate values.
2. Implement specific states if needed.
3. Configure AI with appropriate decisions and actions.
4. Create prefab with required components.

### **10.2. Adding New Abilities**

1. Create new AbilitySO with configuration.
2. Implement new states for the ability.
3. Add input mapping in the Input System.
4. Configure transitions in the Player State Machine.

### **10.3. Adding New Levels**

1. Create new LevelSettingsSO with environment-specific parameters.
2. Build level using existing components and prefabs.
3. Configure enemy spawns and hazards.

## **11. Testing Strategy**

### **11.1. Unit Testing**

* Test individual components in isolation.
* Focus on core systems: State Machine, Event Bus, AI Decisions.

### **11.2. Integration Testing**

* Test interaction between systems.
* Verify event propagation and state transitions.

### **11.3. Playtest Scenarios**

* Define specific scenarios to test during playtesting.
* Focus on edge cases and complex interactions.

## **12. Conclusion**

This architectural design provides a solid foundation for implementing *Petals of Hope* using modern Unity best practices. The modular, event-driven approach with ScriptableObjects for data management ensures the game will be maintainable, extensible, and performant. The state machine system provides a consistent framework for both player and enemy behaviors, while the AI system allows for complex, configurable enemy behaviors without tightly coupled code.

By following this architecture, the development team can work efficiently on different aspects of the game simultaneously, with clear interfaces between systems and minimal dependencies.