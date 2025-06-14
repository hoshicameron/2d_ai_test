using PetalsOfHope.AI.Core;
using UnityEngine;

namespace _Project.Scripts.AI.Actions
{
    /// <summary>
    /// An action node that waits for a specified duration before returning Success.
    /// It returns Running while the timer is active.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Wait")]
    public class WaitNode : ActionNode
    {
        [Tooltip("The amount of time in seconds to wait.")]
        public float duration = 1f;

        [System.NonSerialized]
        private float startTime;
        
        /// <summary>
        /// Creates a deep copy of this node for runtime use.
        /// </summary>
        public override Node Clone()
        {
            return Instantiate(this);
        }

        /// <summary>
        /// Called when the Wait node begins execution.
        /// </summary>
        protected override void OnStart(AIContext context)
        {
            startTime = Time.time;
        }

        /// <summary>
        /// Called when the Wait node stops.
        /// </summary>
        protected override void OnStop(AIContext context) { }

        /// <summary>
        /// Checks the elapsed time and returns the appropriate state.
        /// </summary>
        protected override NodeState OnUpdate(AIContext context)
        {
            if (Time.time - startTime > duration)
            {
                return NodeState.Success;
            }
            return NodeState.Running;
        }
    }
}