using UnityEditor;
using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(Vector3EventSO))]
    public class Vector3EventSOEditor : TypedEventSOEditor<Vector3EventSO, Vector3>
    {
    }
}
