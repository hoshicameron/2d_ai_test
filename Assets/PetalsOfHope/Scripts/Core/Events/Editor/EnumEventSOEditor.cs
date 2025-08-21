using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace PetalsOfHope.Editor.Events
{
    /// <summary>
    /// A generic base editor for any TypedEventSO<T> where T is an Enum.
    /// Provides a debug "Raise" button with an Enum dropdown.
    /// </summary>
    public class EnumEventSOEditor<TEvent, TEnum> : UnityEditor.Editor 
        where TEvent : ScriptableObject 
        where TEnum : Enum
    {
        private TEnum _payloadValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug Raise (Play Mode Only)", EditorStyles.boldLabel);

            GUI.enabled = Application.isPlaying;

            // Draw the enum field for the payload
            _payloadValue = (TEnum)EditorGUILayout.EnumPopup("Payload Value:", _payloadValue);

            if (GUILayout.Button("Raise Event with Payload"))
            {
                if (Application.isPlaying)
                {
                    var targetEvent = (TEvent)target;
                    MethodInfo raiseMethod = typeof(TEvent).GetMethod("Raise", new[] { typeof(TEnum) });
                    if (raiseMethod != null)
                    {
                        raiseMethod.Invoke(targetEvent, new object[] { _payloadValue });
                    }
                    else
                    {
                        Debug.LogError($"Could not find a public 'Raise({typeof(TEnum).Name})' method on {typeof(TEvent).Name}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Cannot raise {typeof(TEvent).Name} from editor when not in Play Mode.");
                }
            }
            GUI.enabled = true;
        }
    }
}
