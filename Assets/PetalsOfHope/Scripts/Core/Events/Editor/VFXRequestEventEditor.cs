using PetalsOfHope.Contracts;
using PetalsOfHope.Core.Events;
using UnityEditor;
using UnityEngine;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(VFXRequestEvent))]
    public class VFXRequestEventEditor : UnityEditor.Editor
    {
        private VFXType _vfxType;
        private Vector3 _position;
        private Quaternion _rotation = Quaternion.identity;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("VFX Test Event", EditorStyles.boldLabel);

            _vfxType = (VFXType)EditorGUILayout.EnumPopup("VFX Type", _vfxType);
            _position = EditorGUILayout.Vector3Field("Position", _position);
            _rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", _rotation.eulerAngles));

            if (GUILayout.Button("Raise Test Event"))
            {
                if (Application.isPlaying)
                {
                    VFXRequestEvent vfxEvent = (VFXRequestEvent)target;
                    vfxEvent.Raise(new VFXRequestData(_vfxType, _position, _rotation));
                }
                else
                {
                    Debug.LogWarning("Test events can only be raised in Play Mode.");
                }
            }
        }
    }
}
