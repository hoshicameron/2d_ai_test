# State Machine System

A flexible and reusable state machine system for Unity.

## Features

- Simple and intuitive API
- Support for both Update and FixedUpdate loops
- Easy state transitions
- Clean separation of concerns
- Debug logging for state changes
- MonoBehaviour integration

## Getting Started

1. **Create a new state** by inheriting from `BaseState`:

```csharp
using UnityEngine;
using PetalsOfHope.Core.StateMachine;

public class YourCustomState : BaseState
{
    public YourCustomState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Called when entering this state
        Debug.Log("Entered YourCustomState");
    }

    public override void Exit()
    {
        // Called when exiting this state
        Debug.Log("Exited YourCustomState");
    }

    public override void Update()
    {
        // Called every frame
    }

    public override void FixedUpdate()
    {
        // Called every fixed frame (for physics)
    }
}
```

2. **Set up the StateMachine** on a GameObject:

```csharp
using UnityEngine;
using PetalsOfHope.Core.StateMachine;

public class YourController : MonoBehaviour
{
    private StateMachine _stateMachine;
    private YourCustomState _customState;
    private AnotherState _anotherState;

    private void Start()
    {
        // Get or add the StateMachine component
        _stateMachine = StateMachine.GetOrAddStateMachine(gameObject);
        
        // Create state instances
        _customState = new YourCustomState(_stateMachine);
        _anotherState = new AnotherState(_stateMachine);
        
        // Initialize with the starting state
        _stateMachine.Initialize(_customState);
    }

    private void Update()
    {
        // Example: Change state on key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_stateMachine.CurrentState == _customState)
            {
                _stateMachine.ChangeState(_anotherState);
            }
            else
            {
                _stateMachine.ChangeState(_customState);
            }
        }
    }
}
```

## Best Practices

1. **State Design**:
   - Keep states focused on a single responsibility
   - Move shared logic to separate components if needed
   - Use the constructor to inject dependencies

2. **Performance**:
   - Cache frequently used components
   - Avoid expensive operations in Update/FixedUpdate
   - Use object pooling for state transitions if creating/destroying states frequently

3. **Debugging**:
   - The system logs state changes by default
   - You can check the current state through the `CurrentState` property

## Example

See `StateMachineExample.cs` in the Examples folder for a complete working example with three states (Idle, Walking, Jumping) that cycle on spacebar press.

## License

This system is part of the Petals of Hope project. See the project's main LICENSE file for details.
