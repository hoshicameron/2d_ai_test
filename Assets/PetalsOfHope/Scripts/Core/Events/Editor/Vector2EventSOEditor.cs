using UnityEditor;
using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(Vector2EventSO))]
    public class Vector2EventSOEditor : TypedEventSOEditor<Vector2EventSO, Vector2>
    {
    }
}
