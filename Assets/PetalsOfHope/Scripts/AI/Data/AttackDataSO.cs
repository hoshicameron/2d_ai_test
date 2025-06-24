using UnityEngine;

[CreateAssetMenu(menuName = "AI/Data/Attack Data")]
public class AttackDataSO : ScriptableObject
{
    [Header("Attack Parameters")]
    [Tooltip("The range at which the AI can initiate an attack.")]
    public float attackRange = 1.5f;
}