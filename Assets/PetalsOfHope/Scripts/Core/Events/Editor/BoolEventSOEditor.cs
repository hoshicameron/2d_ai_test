using UnityEditor;
using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(BoolEventSO))]
    public class BoolEventSOEditor : TypedEventSOEditor<BoolEventSO, bool>
    {
    }
}
