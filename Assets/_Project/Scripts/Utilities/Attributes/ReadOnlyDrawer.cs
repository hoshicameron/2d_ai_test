using UnityEditor;
using UnityEngine;

namespace PetalsOfHope.Utilities
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Save the previous GUI enabled state
            bool previousGUIState = GUI.enabled;
            
            // Disable editing for this property
            GUI.enabled = false;
            
            // Draw the property as normal, but it will be disabled
            EditorGUI.PropertyField(position, property, label, true);
            
            // Restore the previous GUI state
            GUI.enabled = previousGUIState;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
