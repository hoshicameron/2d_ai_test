using _Project.Scripts.Gameplay.Character;
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
        public readonly CharacterControllerBase CharacterController; // To read stats like IsGrounded
        public readonly AIInputSource InputSource;

        // --- Environmental/World Information ---
        public Transform PlayerTransform; // Will be updated by the runner

        // --- Add any other data your AI might need ---
        // public float CurrentHealth;
        // public bool IsInCover;
        // public Vector3 LastKnownPlayerPosition;

        public AIContext(GameObject agent, CharacterControllerBase controller, AIInputSource input)
        {
            Agent = agent;
            AgentTransform = agent.transform;
            CharacterController = controller;
            InputSource = input;
        }
    }
}