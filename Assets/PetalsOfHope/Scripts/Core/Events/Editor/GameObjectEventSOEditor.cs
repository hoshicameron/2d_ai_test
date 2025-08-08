using UnityEditor;
using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(GameObjectEventSO))]
    public class GameObjectEventSOEditor : TypedEventSOEditor<GameObjectEventSO, GameObject>
    {
    }
}
