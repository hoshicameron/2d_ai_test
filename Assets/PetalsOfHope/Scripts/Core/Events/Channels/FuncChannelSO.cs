using System;
using UnityEngine;

namespace PetalsOfHope.Core.Events.Channels
{
    /// <summary>
    /// A generic base class for a function channel ScriptableObject.
    /// This pattern allows one system to provide a function that other systems can consume
    /// without a direct reference, returning a value immediately.
    /// </summary>
    /// <typeparam name="TIn">The input parameter type of the function.</typeparam>
    /// <typeparam name="TOut">The return type of the function.</typeparam>
    public abstract class FuncChannelSO<TIn, TOut> : ScriptableObject
    {
        /// <summary>
        /// The function to be provided by a system (e.g., GameProgressionSystem).
        /// It takes a parameter of type TIn and returns a value of type TOut.
        /// </summary>
        public Func<TIn, TOut> Function;
    }
}
