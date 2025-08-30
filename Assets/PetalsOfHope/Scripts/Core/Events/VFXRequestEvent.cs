using PetalsOfHope.Contracts;
using UnityEngine;

namespace PetalsOfHope.Core.Events
{
    [CreateAssetMenu(fileName = "VFXRequestEvent", menuName = "Petals of Hope/Systems/VFX/VFX Request Event")]
    public class VFXRequestEvent : TypedEventSO<IVFXRequestData>
    {
    }
}
