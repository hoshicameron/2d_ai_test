using UnityEngine;
using PetalsOfHope.Data.Abilities;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Wall Grab")]
    public class WallGrabSO : AbilitySO
    {
        [Header("Wall Grab Settings")]
        [Min(0.1f), Tooltip("How fast the player slides down the wall while grabbing")]
        public float wallSlideSpeed = 2f;
        
        [Min(0.1f), Tooltip("How long the player can grab the wall before sliding starts")]
        public float wallGrabTime = 0.5f;
        
        [Tooltip("Layer mask for what counts as a wall")]
        public LayerMask wallLayer;
        
        [Tooltip("Offset from player's center for wall detection")]
        public Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
        
        [Tooltip("Size of the wall check box")]
        public Vector2 wallCheckSize = new Vector2(0.1f, 0.8f);

        public WallGrabSO()
        {
            abilityName = "WallGrab";
            description = "Allows the player to grab and slide down walls.";
        }
    }
}
