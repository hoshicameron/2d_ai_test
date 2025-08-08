using UnityEditor;
using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(FloatEventSO))]
    public class FloatEventSOEditor : TypedEventSOEditor<FloatEventSO, float>
    {
    }
}
