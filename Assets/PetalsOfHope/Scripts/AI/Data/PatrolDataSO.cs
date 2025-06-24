using UnityEngine;

/// <summary>
/// A ScriptableObject that holds configuration data for an AI's patrol behavior.
/// This allows designers to easily create and tweak different patrol styles.
/// </summary>
[CreateAssetMenu(fileName ="New PatrolDataSO" ,menuName = "AI/Data/Patrol Data")]
public class PatrolDataSO : ScriptableObject
{
    [Header("Detection Settings")]
    [Tooltip("The layer(s) that should be considered a wall.")]
    public LayerMask wallLayer;

    [Tooltip("The layer(s) that should be considered ground.")]
    public LayerMask groundLayer;

    [Tooltip("How far from the agent's pivot to check for a wall in front.")]
    public float wallCheckDistance = 0.5f;

    [Tooltip("How far in front of the agent to check for a ledge.")]
    public float ledgeCheckForwardDistance = 0.5f;
        
    [Tooltip("How far down to check for ground from the ledge check point.")]
    public float ledgeCheckDownwardDistance = 1.0f;
}
