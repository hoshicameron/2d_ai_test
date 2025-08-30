using PetalsOfHope.Contracts;
using UnityEngine;

namespace PetalsOfHope.Data.VFX
{
    [CreateAssetMenu(fileName = "VFXData", menuName = "Petals of Hope/Systems/VFX/VFX Data")]
    public class VFXData : ScriptableObject
    {
        [Tooltip("The type of VFX this data represents.")]
        [SerializeField] private VFXType _vfxType;

        [Tooltip("The prefab to instantiate for this VFX.")]
        [SerializeField] private GameObject _vfxPrefab;

        public VFXType VfxType => _vfxType;
        public GameObject VfxPrefab => _vfxPrefab;
    }
}
