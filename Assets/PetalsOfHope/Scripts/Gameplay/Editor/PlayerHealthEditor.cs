using UnityEditor;
using UnityEngine;
using PetalsOfHope.Gameplay.Player;

namespace PetalsOfHope.Editor
{
    [CustomEditor(typeof(PlayerHealth))]
    public class PlayerHealthEditor : UnityEditor.Editor
    {
        private const int SmallButtonWidth = 30;
        private const int MediumButtonWidth = 50;
        
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            
            // Add some space
            EditorGUILayout.Space(10);
            
            // Get a reference to the PlayerHealth component
            PlayerHealth playerHealth = (PlayerHealth)target;
            
            // Create a header for the test section
            EditorGUILayout.LabelField("Test Tools", EditorStyles.boldLabel);
            
            // Create a horizontal group for the damage buttons
            EditorGUILayout.BeginHorizontal();
            
            // Add a button to deal 10 damage
            if (GUILayout.Button("Damage (10)", GUILayout.Width(100)))
            {
                playerHealth.TakeDamage(10);
                EditorUtility.SetDirty(playerHealth);
            }
            
            // Add a button to deal 25 damage
            if (GUILayout.Button("Damage (25)", GUILayout.Width(100)))
            {
                playerHealth.TakeDamage(25);
                EditorUtility.SetDirty(playerHealth);
            }
            
            EditorGUILayout.EndHorizontal();
            
            // Create a horizontal group for the heal and kill buttons
            EditorGUILayout.BeginHorizontal();
            
            // Add a button to heal 10 health
            if (GUILayout.Button("Heal (10)", GUILayout.Width(100)))
            {
                playerHealth.Heal(10);
                EditorUtility.SetDirty(playerHealth);
            }
            
            // Add a button to kill the player
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Kill", GUILayout.Width(100)))
            {
                playerHealth.TakeDamage(playerHealth.CurrentHealth);
                EditorUtility.SetDirty(playerHealth);
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
            
            // Display current health information
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField($"Current Health: {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}");
            
            // Add a horizontal slider to visualize health
            float healthPercentage = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUI.ProgressBar(rect, healthPercentage, $"Health: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}");
            GUILayout.Space(18);
            EditorGUILayout.EndVertical();
        }
    }
}
