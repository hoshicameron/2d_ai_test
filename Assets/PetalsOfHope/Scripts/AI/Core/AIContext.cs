using PetalsOfHope.AI.Data;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Scripts.AI.Data;
using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    /// <summary>
    /// A data container that acts as a "Blackboard" for the Behavior Tree.
    /// It holds all the world-state information that the AI needs to make decisions,
    /// decoupling the AI's "brain" (the tree) from its "body" (the character's components).
    /// </summary>
    public class AIContext
    {
        // --- Core References ---
        public readonly GameObject Agent;
        public readonly Transform AgentTransform;
        public readonly ICharacterController CharacterController; // To read stats like IsGrounded
        public readonly AIInputSource InputSource;
        
        // --- Configuration Data ---
        public readonly PatrolDataSO PatrolData;
        public readonly ChaseDataSO ChaseData; 
        public readonly IdleDataSO IdleData; 
        public readonly AttackDataSO AttackData;

        // --- Environmental/World Information ---
        public Transform PlayerTransform; // Will be updated by the runner
        
        // --- Runtime AI State ---
        public int CurrentFacingDirection = 1;
        public bool IsInitialized = false;
        
        // --- Memory ---
        public Vector3 LastKnownPlayerPosition;
        public float TimeLastSeenPlayer = -99f;

        // --- Add any other data your AI might need ---
        // public float CurrentHealth;
        // public bool IsInCover;
        // public Vector3 LastKnownPlayerPosition;
        
        /// <summary>
        /// A reference to the node that is currently in the 'Running' state.
        /// This is updated every frame by the Behavior Tree.
        /// </summary>
        public Node CurrentRunningNode;

        // Constructor updated to accept PatrolDataSO
        public AIContext(GameObject agent, ICharacterController controller, AIInputSource input,IdleDataSO idleData, PatrolDataSO patrolData, ChaseDataSO chaseData, AttackDataSO attackData)
        {
            Agent = agent;
            AgentTransform = agent.transform;
            CharacterController = controller;
            InputSource = input;
            PatrolData = patrolData;
            ChaseData = chaseData;
            IdleData = idleData;
            AttackData = attackData;
        }
    }
}