using PetalsOfHope.AI.Core;
using PetalsOfHope.AI.Data;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Scripts.AI.Data;
using UnityEditor;
using UnityEngine;

namespace PetalsOfHope.AI
{
    [RequireComponent(typeof(ICharacterController))]
    [RequireComponent(typeof(AIInputSource))]
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [Tooltip("The Behavior Tree asset to run for this agent.")]
        public BehaviorTree treeAsset;
        
        [Tooltip("The data asset that defines the parameters for patrol behavior.")]
        public PatrolDataSO patrolData; // Field for the designer to assign the data
        
        [Tooltip("The data asset that defines the parameters for chase behavior.")]
        public ChaseDataSO chaseData; // Field for the designer to assign the chase data
        
        [Tooltip("Data asset defining idle timings.")]
        public IdleDataSO idleData; // Field for the designer to assign the idle data
        
        [Tooltip("The data asset that defines the parameters for attack behavior.")]
        public AttackDataSO attackData; // Field for the designer to assign the chase data
        
        /// <summary>
        /// A public property to easily see the name of the currently running action node.
        /// Returns "None" if the AI is between actions (e.g., just finished one).
        /// </summary>
        public string CurrentActionNodeName => context?.CurrentRunningNode?.name ?? "None";

        public string RuningNodeName;
        // --- Runtime References ---
        private Node runtimeTree;
        private AIContext context;

        private void Start()
        {
            // Find necessary components
            var characterController = GetComponent<ICharacterController>();
            var aiInputSource = GetComponent<AIInputSource>();
            
            // Create the context that the tree will use to read/write data
            context = new AIContext(gameObject, characterController, aiInputSource,idleData, patrolData, chaseData, attackData);

            // Clone the Behavior Tree asset to create a unique, stateful instance for this agent.
            if (treeAsset != null)
            {
                runtimeTree = treeAsset.Clone();
            }
            else
            {
                Debug.LogError($"BehaviorTreeRunner on {gameObject.name} does not have a Tree Asset assigned.", this);
            }
        }

        private void Update()
        {
            if (runtimeTree == null || context == null) return;
            
            // --- POPULATE THE CONTEXT ---
            // This is where you update the AI's "senses" every frame.
            UpdateContext();
            
            // At the start of each frame, we clear the running state.
            // This ensures that if a node is interrupted, it doesn't stay stuck
            // as the "current" node.
            context.CurrentRunningNode = null;

            // --- TICK THE TREE ---
            // The tree runs its logic based on the fresh context data.
            runtimeTree.Evaluate(context);
        }

        /// <summary>
        /// Gathers up-to-date information from the world and updates the AIContext.
        /// </summary>
        private void UpdateContext()
        {
            // Example: Find the player's transform.
            // In a real game, you might use a more efficient manager to track the player.
            if (context.PlayerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    context.PlayerTransform = player.transform;
                }
            }

            RuningNodeName = CurrentActionNodeName;

            // Add other updates here, e.g.:
            // context.CurrentHealth = healthComponent.Health;
            // context.LastKnownPlayerPosition = perceptionSystem.GetPlayerPosition();
        }
        
        /// <summary>
        /// When the agent is selected in the editor, traverse the behavior tree
        /// and draw the gizmos for each node.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying || runtimeTree == null || context == null)
            {
                return;
            }
            
            // Start the recursive drawing process from the root node.
            TraverseAndDraw(runtimeTree);
            
            // --- New Label Drawing (Wrapped for safety) ---
            // The code that uses the Handles class is now wrapped in a preprocessor directive.
#if UNITY_EDITOR
            string label = $"Current Action: {CurrentActionNodeName}";
            Handles.color = Color.white;
            Handles.Label(transform.position + Vector3.up * 1.0f, label);
#endif
        }

        /// <summary>
        /// Recursively traverses the tree and calls the DrawGizmos method on each node.
        /// </summary>
        private void TraverseAndDraw(Node node)
        {
            if (node == null) return;

            // Tell the current node to draw its specific gizmo.
            node.DrawGizmos(context);

            // If the node has children, continue traversing.
            if (node is CompositeNode composite)
            {
                foreach (var child in composite.children)
                {
                    TraverseAndDraw(child);
                }
            }
            else if (node is DecoratorNode decorator)
            {
                TraverseAndDraw(decorator.child);
            }
        }
    }
}