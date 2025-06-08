using UnityEditor;
using UnityEngine;
using PetalsOfHope.Gameplay.Enemies.Core;

namespace PetalsOfHope.Editor
{
    [CustomEditor(typeof(EnemyHealth))]
    [CanEditMultipleObjects]
    public class EnemyHealthEditor : UnityEditor.Editor
    {
        private const int ButtonWidth = 100;
        private const int SmallButtonWidth = 80;
        
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            
            // Add some space
            EditorGUILayout.Space(10);
            
            // Get a reference to the EnemyHealth component
            EnemyHealth enemyHealth = (EnemyHealth)target;
            
            // Early exit if the editor is in play mode and the enemy is dead
            if (Application.isPlaying && enemyHealth.IsDead)
            {
                EditorGUILayout.HelpBox("Enemy is dead!", MessageType.Warning);
                return;
            }
            
            // Create a header for the test section
            EditorGUILayout.LabelField("Test Tools", EditorStyles.boldLabel);
            
            // Display current health information
            EditorGUILayout.LabelField($"Current Health: {enemyHealth.CurrentHealth} / {enemyHealth.MaxHealth}");
            
            // Health bar
            float healthPercentage = enemyHealth.MaxHealth > 0 
                ? (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth 
                : 0f;
                
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUI.ProgressBar(rect, healthPercentage, $"Health: {enemyHealth.CurrentHealth}/{enemyHealth.MaxHealth}");
            GUILayout.Space(18);
            EditorGUILayout.EndVertical();
            
            // Only show test buttons in play mode
            if (Application.isPlaying)
            {
                // Damage buttons
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Damage (10)", GUILayout.Width(ButtonWidth)))
                {
                    enemyHealth.TakeDamage(10);
                    EditorUtility.SetDirty(enemyHealth);
                }
                
                if (GUILayout.Button("Damage (25)", GUILayout.Width(ButtonWidth)))
                {
                    enemyHealth.TakeDamage(25);
                    EditorUtility.SetDirty(enemyHealth);
                }
                
                EditorGUILayout.EndHorizontal();
                
                // Kill and reset buttons
                EditorGUILayout.BeginHorizontal();
                
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Kill", GUILayout.Width(SmallButtonWidth)))
                {
                    enemyHealth.TakeDamage(enemyHealth.CurrentHealth);
                    EditorUtility.SetDirty(enemyHealth);
                }
                GUI.backgroundColor = Color.white;
                
                if (GUILayout.Button("Reset Health", GUILayout.Width(ButtonWidth)))
                {
                    System.Reflection.MethodInfo resetMethod = typeof(EnemyHealth).GetMethod("ResetHealth", 
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    
                    if (resetMethod != null)
                    {
                        resetMethod.Invoke(enemyHealth, null);
                        EditorUtility.SetDirty(enemyHealth);
                    }
                }
                
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox("Enter Play Mode to test health functionality.", MessageType.Info);
            }
        }
    }
}
