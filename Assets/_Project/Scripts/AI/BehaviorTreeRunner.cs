using _Project.Scripts.Gameplay.Character;
using PetalsOfHope.AI.Core;
using UnityEngine;

namespace _Project.Scripts.AI
{
    [RequireComponent(typeof(CharacterControllerBase))]
    [RequireComponent(typeof(AIInputSource))]
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [Tooltip("The Behavior Tree asset to run for this agent.")]
        public BehaviorTree treeAsset;

        // --- Runtime References ---
        private Node runtimeTree;
        private AIContext context;

        private void Start()
        {
            // Find necessary components
            var characterController = GetComponent<CharacterControllerBase>();
            var aiInputSource = GetComponent<AIInputSource>();
            
            // Create the context that the tree will use to read/write data
            context = new AIContext(gameObject, characterController, aiInputSource);

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
            
            // Add other updates here, e.g.:
            // context.CurrentHealth = healthComponent.Health;
            // context.LastKnownPlayerPosition = perceptionSystem.GetPlayerPosition();
        }
    }
}