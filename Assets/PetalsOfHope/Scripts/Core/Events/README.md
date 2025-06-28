# Event Bus System

A ScriptableObject-based event system for decoupled communication between different game systems in Unity.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Basic Usage](#basic-usage)
- [Event Types](#event-types)
  - [GameEventSO](#gameeventso)
  - [TypedEventSO<T>](#typedeventsot)
- [Listeners](#listeners)
  - [EventListener](#eventlistener)
  - [TypedEventListener](#typedeventlistener)
- [Editor Integration](#editor-integration)
- [Best Practices](#best-practices)
- [Examples](#examples)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Event Bus System provides a decoupled way for different parts of your game to communicate without direct references. It's built on top of Unity's ScriptableObjects and supports both parameterless events and typed events with payloads.

## Features

- **Decoupled Communication**: Systems can communicate without direct references
- **Type Safety**: Strongly-typed events with generic support
- **Editor Integration**: Custom inspectors and menu items for easy workflow
- **Flexible**: Supports both code-based and UnityEvent-based listeners
- **Testable**: Includes unit tests for core functionality

## Getting Started

### Installation

1. Import the event system scripts into your Unity project
2. Make sure the `PetalsOfHope.Runtime` assembly definition references any required Unity assemblies
3. The editor scripts require the `PetalsOfHope.Editor` assembly definition

### Basic Usage

1. **Create an Event Asset**:
   - Right-click in the Project window
   - Select `Create > Petals of Hope/Events/Game Event` for parameterless events
   - Or select a specific typed event from `Create > Petals of Hope/Events/Typed Events/`

2. **Raise an Event**:
   ```csharp
   // In any script with a reference to the event asset
   [SerializeField] private GameEventSO myEvent;
   
   public void SomethingHappened()
   {
       myEvent.Raise();
   }
   ```

3. **Listen to an Event**:
   - **In Code**:
     ```csharp
     private void OnEnable()
     {
         myEvent.RegisterListener(MyResponseMethod);
     }
     
     private void OnDisable()
     {
         myEvent.UnregisterListener(MyResponseMethod);
     }
     
     private void MyResponseMethod()
     {
         Debug.Log("Event was raised!");
     }
     ```
   - **In Inspector**:
     1. Add an `EventListener` component to a GameObject
     2. Drag your event asset to the `Event SO` field
     3. Add a response in the `On Event Raised` UnityEvent

## Event Types

### GameEventSO

Parameterless events for simple notifications.

**Create Asset**: `Create > Petals of Hope/Events/Game Event`

**Usage**:
```csharp
// Raise the event
myEvent.Raise();

// Listen to the event
myEvent.RegisterListener(MyMethod);
myEvent.UnregisterListener(MyMethod);
```

### TypedEventSO<T>

Generic events that can carry a payload of any type.

**Available Types**:
- `IntEventSO` - Carries an integer payload
- `FloatEventSO` - Carries a float payload
- `StringEventSO` - Carries a string payload
- `BoolEventSO` - Carries a boolean payload
- `Vector2EventSO` - Carries a Vector2 payload
- `Vector3EventSO` - Carries a Vector3 payload
- `GameObjectEventSO` - Carries a GameObject payload

**Create Asset**: `Create > Petals of Hope/Events/Typed Events/[Type] Event`

**Usage**:
```csharp
// Raise the event with a payload
myTypedEvent.Raise(42);

// Listen to the event
myTypedEvent.RegisterListener(MyTypedMethod);
myTypedEvent.UnregisterListener(MyTypedMethod);

private void MyTypedMethod(int value)
{
    Debug.Log($"Received value: {value}");
}
```

## Listeners

### EventListener

A MonoBehaviour component that listens to a `GameEventSO` and invokes a UnityEvent in response.

**Add to GameObject**: `Add Component > Petals of Hope/Event Listeners/Event Listener`

**Properties**:
- `Event SO`: The GameEventSO to listen to
- `On Event Raised`: UnityEvent that gets invoked when the event is raised

### TypedEventListener

A generic MonoBehaviour component that listens to a `TypedEventSO<T>` and invokes a UnityEvent<T> in response.

**Available Types**:
- `Int Event Listener`
- `Float Event Listener`
- `String Event Listener`
- `Bool Event Listener`
- `Vector2 Event Listener`
- `Vector3 Event Listener`
- `GameObject Event Listener`

**Add to GameObject**: `Add Component > Petals of Hope/Event Listeners/[Type] Event Listener`

**Properties**:
- `Event SO`: The TypedEventSO to listen to
- `On Event Raised`: UnityEvent that gets invoked with the payload when the event is raised

## Editor Integration

### Debugging

- In Play Mode, select any event asset to see a "Raise Event" button in the Inspector
- For typed events, you can specify a test payload to send

### Creating Event Assets

You can quickly create all commonly used event assets by going to:
`Petals of Hope > Create Event Assets`

This will create a set of predefined events in `Assets/_Project/ScriptableObjects/Events/`

## Best Practices

1. **Naming Conventions**:
   - Event assets should end with "EventSO" (e.g., `PlayerJumpedEventSO`)
   - Use descriptive names that indicate when the event is raised

2. **Memory Management**:
   - Always unregister listeners in `OnDisable` or `OnDestroy`
   - Be careful with anonymous methods when registering listeners

3. **Performance**:
   - Avoid raising events every frame if possible
   - Consider using direct method calls for high-frequency communication

4. **Debugging**:
   - Use the developer description field to document the event's purpose
   - The editor tools include buttons to manually raise events for testing

## Examples

### Creating and Raising a Custom Event

1. Create a new event asset: `Create > Petals of Hope/Events/Game Event`
2. Name it `PlayerJumpedEventSO`
3. In your player controller:
   ```csharp
   [SerializeField] private GameEventSO playerJumpedEvent;
   
   private void Update()
   {
       if (Input.GetButtonDown("Jump") && IsGrounded())
       {
           Jump();
           playerJumpedEvent.Raise();
       }
   }
   ```

4. To listen to this event:
   - Add an `AudioSource` component to your player
   - Add an `EventListener` component
   - Assign the `PlayerJumpedEventSO` asset
   - Add a response to play a jump sound

### Using Typed Events

1. Create a typed event: `Create > Petals of Hope/Events/Typed Events/Int Event`
2. Name it `PlayerHealthChangedEventSO`
3. In your player health script:
   ```csharp
   [SerializeField] private IntEventSO playerHealthChangedEvent;
   
   public void TakeDamage(int amount)
   {
       currentHealth = Mathf.Max(0, currentHealth - amount);
       playerHealthChangedEvent.Raise(currentHealth);
       
       if (currentHealth <= 0)
       {
           Die();
       }
   }
   ```

4. To update the UI when health changes:
   - Add an `Int Event Listener` to your health bar UI
   - Assign the `PlayerHealthChangedEventSO` asset
   - Add a response that updates the health bar fill amount

## Testing

The event system includes unit tests to ensure proper functionality. To run the tests:

1. Open the Test Runner window: `Window > General > Test Runner`
2. Select the EditMode tab
3. Click "Run All" to execute all tests

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
