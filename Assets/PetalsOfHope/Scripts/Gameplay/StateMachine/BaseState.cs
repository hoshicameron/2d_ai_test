namespace PetalsOfHope.Gameplay.StateMachine
{
    /// <summary>
    /// Abstract base class for all states in the state machine.
    /// Inherit from this class to create concrete state implementations.
    /// </summary>
    public abstract class BaseState
    {
        protected readonly StateMachine _stateMachine;

        /// <summary>
        /// Initializes a new instance of the BaseState class.
        /// </summary>
        /// <param name="stateMachine">The state machine this state belongs to.</param>
        protected BaseState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        /// <summary>
        /// Called when the state machine enters this state.
        /// </summary>
        public abstract void Enter();

        /// <summary>
        /// Called when the state machine exits this state.
        /// </summary>
        public abstract void Exit();

        /// <summary>
        /// Called every frame while this state is active.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called every fixed frame while this state is active.
        /// Use this for physics-related calculations.
        /// </summary>
        public abstract void FixedUpdate();
    }
}
