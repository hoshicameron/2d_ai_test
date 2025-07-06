using System;
using UnityEngine;

namespace PetalsOfHope.Data.Abilities
{
    // These are simple data containers, not ScriptableObjects themselves.
    // The [Serializable] attribute allows them to be displayed and edited in the Inspector.

    [Serializable]
    public class MoveData
    {
        public bool isEnabled = true; // Movement is enabled by default
        [Min(0f)] public float movementSpeed = 5f;
    }

    [Serializable]
    public class FallData
    {
        public bool isEnabled = true; // Falling is enabled by default
        public float gravityScale = 2.5f;
    }

    [Serializable]
    public class JumpData
    {
        public bool isEnabled;
        [Min(0f)] public float jumpForce = 10f;
        [Range(0f, 1f)] public float airControlFactor = 0.8f;
        [Min(1)] public int maxJumps = 1;
        [Min(0f)] public float doubleJumpForceMultiplier = 1.0f;
    }

    [Serializable]
    public class DashData
    {
        public bool isEnabled;
        [Min(0.1f)] public float dashSpeed = 20f;
        [Min(0.05f)] public float dashDuration = 0.2f;
        [Min(0f)] public float cooldown = 1.5f;
    }

    [Serializable]
    public class WallGrabData
    {
        public bool isEnabled;
        [Min(0.1f)] public float wallSlideSpeed = 2f;
        [Min(0.1f)] public float wallGrabTime = 0.5f;
        public LayerMask wallLayer;
        public Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
        public Vector2 wallCheckSize = new Vector2(0.1f, 0.8f);
    }

    [Serializable]
    public class WallJumpData
    {
        public bool isEnabled;
        [Min(1f)] public float wallJumpForce = 15f;
        [Min(1f)] public float wallJumpHorizontalForce = 10f;
        [Min(0f)] public float wallJumpInputDisableTime = 0.2f;
        [Min(0f)] public float coyoteWallTime = 0.2f;
    }

    [Serializable]
    public class ClimbData
    {
        public bool isEnabled;
        [Min(0.1f)] public float climbSpeed = 3f;
        [Min(0.1f)] public float climbAcceleration = 15f;
        [Min(0.1f)] public float climbDeceleration = 20f;
        public LayerMask climbableLayer;
        public Vector2 ladderCheckSize = new Vector2(0.5f, 0.1f);
        [Range(0f, 1f)] public float horizontalSnapDistance = 0.1f;
    }


    /// <summary>
    /// A master ScriptableObject that contains all possible ability data for a character.
    /// Abilities can be enabled or disabled via the 'isEnabled' checkbox.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Character Ability Sheet")]
    public class AbilitySheetSO : ScriptableObject
    {
        [Header("Core Abilities")]
        public MoveData moveData = new MoveData();
        public FallData fallData = new FallData();
        
        [Header("Optional Abilities")]
        public JumpData jumpData = new JumpData();
        public DashData dashData = new DashData();
        public WallGrabData wallGrabData = new WallGrabData();
        public WallJumpData wallJumpData = new WallJumpData();
        public ClimbData climbData = new ClimbData();
    }
}