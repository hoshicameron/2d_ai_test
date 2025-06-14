using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: Node.cs
    // =========================================================================================
    public enum NodeState { Running, Success, Failure }

    public abstract class Node : ScriptableObject
    {
        [System.NonSerialized] protected NodeState state = NodeState.Failure;
        [System.NonSerialized] protected bool started = false;
        
        [Tooltip("A unique identifier for this node.")]
        public string guid;
        [TextArea] public string description;

        public NodeState Evaluate(AIContext context)
        {
            if (!started)
            {
                OnStart(context);
                started = true;
            }

            state = OnUpdate(context);

            if (state != NodeState.Running)
            {
                OnStop(context);
                started = false;
            }
            return state;
        }

        public abstract Node Clone();

        protected virtual void OnStart(AIContext context) { }
        protected virtual void OnStop(AIContext context) { }
        protected abstract NodeState OnUpdate(AIContext context);
    }
}