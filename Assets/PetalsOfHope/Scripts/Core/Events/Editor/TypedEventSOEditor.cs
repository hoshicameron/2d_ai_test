using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace PetalsOfHope.Editor.Events
{
    /// <summary>
    /// A generic base editor for any TypedEventSO<T> to provide a debug "Raise" button.
    /// </summary>
    public class TypedEventSOEditor<TEvent, TPayload> : UnityEditor.Editor where TEvent : ScriptableObject
    {
        private TPayload _payloadValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug Raise (Play Mode Only)", EditorStyles.boldLabel);

            GUI.enabled = Application.isPlaying;

            // Draw the payload field
            _payloadValue = DrawPayloadField(_payloadValue);

            if (GUILayout.Button("Raise Event with Payload"))
            {
                if (Application.isPlaying)
                {
                    var targetEvent = (TEvent)target;
                    MethodInfo raiseMethod = typeof(TEvent).GetMethod("Raise", new[] { typeof(TPayload) });
                    if (raiseMethod != null)
                    {
                        raiseMethod.Invoke(targetEvent, new object[] { _payloadValue });
                    }
                    else
                    {
                        Debug.LogError($"Could not find a public 'Raise({typeof(TPayload).Name})' method on {typeof(TEvent).Name}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Cannot raise {typeof(TEvent).Name} from editor when not in Play Mode.");
                }
            }
            GUI.enabled = true;
        }

        private TPayload DrawPayloadField(TPayload currentValue)
        {
            if (typeof(TPayload) == typeof(int))
            {
                return (TPayload)(object)EditorGUILayout.IntField("Payload Value:", (int)(object)currentValue);
            }
            if (typeof(TPayload) == typeof(float))
            {
                return (TPayload)(object)EditorGUILayout.FloatField("Payload Value:", (float)(object)currentValue);
            }
            if (typeof(TPayload) == typeof(bool))
            {
                return (TPayload)(object)EditorGUILayout.Toggle("Payload Value:", (bool)(object)currentValue);
            }
            if (typeof(TPayload) == typeof(string))
            {
                return (TPayload)(object)EditorGUILayout.TextField("Payload Value:", (string)(object)currentValue);
            }
            if (typeof(TPayload) == typeof(Vector2))
            {
                return (TPayload)(object)EditorGUILayout.Vector2Field("Payload Value:", (Vector2)(object)currentValue);
            }
            if (typeof(TPayload) == typeof(Vector3))
            {
                return (TPayload)(object)EditorGUILayout.Vector3Field("Payload Value:", (Vector3)(object)currentValue);
            }
            if (typeof(TPayload).IsSubclassOf(typeof(UnityEngine.Object)))
            {
                 return (TPayload)(object)EditorGUILayout.ObjectField("Payload Value:", (UnityEngine.Object)(object)currentValue, typeof(TPayload), true);
            }

            EditorGUILayout.LabelField("Payload type not supported for editor raising.");
            return currentValue;
        }
    }
}
