// In Assets/_Project/Editor/Events/GameEventSOEditor.cs
namespace PetalsOfHope.Editor.Events
{
    using UnityEditor;
    using UnityEngine;
    using PetalsOfHope.Core.Events;

    [CustomEditor(typeof(GameEventSO))]
    public class GameEventSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // Draw default inspector

            GUI.enabled = Application.isPlaying; // Enable button only in Play Mode

            GameEventSO gameEvent = (GameEventSO)target;
            if (GUILayout.Button("Raise Event"))
            {
                if (Application.isPlaying)
                {
                    gameEvent.Raise();
                }
                else
                {
                    Debug.LogWarning("Cannot raise GameEventSO from editor when not in Play Mode. Start the game to test event raising.");
                }
            }
            GUI.enabled = true;
        }
    }
}
