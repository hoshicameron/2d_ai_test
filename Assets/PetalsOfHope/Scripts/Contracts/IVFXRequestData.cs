
using UnityEngine;

namespace PetalsOfHope.Contracts
{
    public interface IVFXRequestData
    {
        VFXType VfxType { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
    }
}
