using PetalsOfHope.Contracts;
using UnityEngine;

namespace PetalsOfHope.Core.Events
{
    public struct VFXRequestData : IVFXRequestData
    {
        public VFXType VfxType { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public VFXRequestData(VFXType vfxType, Vector3 position, Quaternion rotation)
        {
            VfxType = vfxType;
            Position = position;
            Rotation = rotation;
        }
    }
}
