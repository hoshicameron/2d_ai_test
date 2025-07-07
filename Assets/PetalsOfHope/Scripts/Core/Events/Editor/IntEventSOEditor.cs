// In Assets/_Project/Editor/Events/IntEventSOEditor.cs
namespace PetalsOfHope.Editor.Events
{
    using UnityEditor;
    using UnityEngine;
    using PetalsOfHope.Core.Events;

    [CustomEditor(typeof(IntEventSO))]
    public class IntEventSOEditor : UnityEditor.Editor
    {
        private int _payloadValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug Raise (Play Mode Only)", EditorStyles.boldLabel);

            GUI.enabled = Application.isPlaying;

            _payloadValue = EditorGUILayout.IntField("Payload Value:", _payloadValue);

            if (GUILayout.Button("Raise Event with Payload"))
            {
                if (Application.isPlaying)
                {
                    var typedEvent = (IntEventSO)target;
                    typedEvent.Raise(_payloadValue);
                }
                else
                {
                    Debug.LogWarning("Cannot raise IntEventSO from editor when not in Play Mode. Start the game to test event raising.");
                }
            }
            GUI.enabled = true;
        }
    }
}
