using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using Action = System.Action;

namespace PetalsOfHope.Contracts
{
    public interface ICharacterInputSource
    {
        Action<Vector2> MoveEvent { get; set;}
        Action JumpEvent { get; set;}
        Action JumpCancelledEvent { get; set;}
        Action DashEvent { get; set; }
        Action AttackEvent { get; set;}
        Action InteractEvent { get; set;}
        
        void DisableAllInput();
        void EnableGameplayInput();
    }
}