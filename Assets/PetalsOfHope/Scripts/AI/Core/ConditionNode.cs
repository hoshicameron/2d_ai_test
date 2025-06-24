namespace PetalsOfHope.AI.Core
{
    /// <summary>
    /// An abstract base class for leaf nodes that check a condition in the game world.
    /// Conditions are the "senses" of the AI and should execute quickly,
    /// typically returning Success or Failure in a single frame.
    /// </summary>
    public abstract class ConditionNode : Node
    {
        /// <summary>
        /// Creates a deep copy of this node for runtime use.
        /// </summary>
        public override Node Clone()
        {
            // For most simple conditions, a direct instantiation is sufficient.
            return Instantiate(this);
        }

        // Note: We don't override OnStart or OnStop. Conditions are typically
        // instantaneous, so they rarely need setup or cleanup logic.
        // Child classes are free to override them if necessary.
    }
}