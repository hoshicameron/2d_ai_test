using UnityEditor;
using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(StringEventSO))]
    public class StringEventSOEditor : TypedEventSOEditor<StringEventSO, string>
    {
    }
}
