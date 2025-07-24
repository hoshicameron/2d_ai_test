using System.Collections.Generic;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Animation
{
    /// <summary>
    /// A reusable wrapper around Unity's Animator component that provides a simplified interface
    /// for playing animations and setting parameters, with automatic parameter hashing for performance.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private readonly Dictionary<string, int> _parameterHashMap = new Dictionary<string, int>();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("AnimationController requires an Animator component on the same GameObject.", this);
                enabled = false;
            }
        }

        #region Public Methods
        
        /// <summary>
        /// Plays an animation state by name.
        /// </summary>
        public void Play(string stateName)
        {
            if (_animator == null) return;
            _animator.Play(stateName);
        }

        /// <summary>
        /// Plays an animation state using its hash for better performance.
        /// </summary>
        public void Play(int stateNameHash)
        {
            if (_animator == null) return;
            _animator.Play(stateNameHash);
        }

        /// <summary>
        /// Sets a boolean parameter on the Animator.
        /// </summary>
        public void SetBool(string paramName, bool value)
        {
            if (_animator == null) return;
            _animator.SetBool(GetParamHash(paramName), value);
        }

        /// <summary>
        /// Sets a boolean parameter on the Animator using its hash.
        /// </summary>
        public void SetBool(int paramHash, bool value)
        {
            if (_animator == null) return;
            _animator.SetBool(paramHash, value);
        }

        /// <summary>
        /// Sets a float parameter on the Animator.
        /// </summary>
        public void SetFloat(string paramName, float value)
        {
            if (_animator == null) return;
            _animator.SetFloat(GetParamHash(paramName), value);
        }

        /// <summary>
        /// Sets a float parameter on the Animator using its hash.
        /// </summary>
        public void SetFloat(int paramHash, float value)
        {
            if (_animator == null) return;
            _animator.SetFloat(paramHash, value);
        }

        /// <summary>
        /// Sets an integer parameter on the Animator.
        /// </summary>
        public void SetInteger(string paramName, int value)
        {
            if (_animator == null) return;
            _animator.SetInteger(GetParamHash(paramName), value);
        }

        /// <summary>
        /// Sets an integer parameter on the Animator using its hash.
        /// </summary>
        public void SetInteger(int paramHash, int value)
        {
            if (_animator == null) return;
            _animator.SetInteger(paramHash, value);
        }

        /// <summary>
        /// Sets a trigger parameter on the Animator.
        /// </summary>
        public void SetTrigger(string paramName)
        {
            if (_animator == null) return;
            _animator.SetTrigger(GetParamHash(paramName));
        }

        /// <summary>
        /// Sets a trigger parameter on the Animator using its hash.
        /// </summary>
        public void SetTrigger(int paramHash)
        {
            if (_animator == null) return;
            _animator.SetTrigger(paramHash);
        }

        /// <summary>
        /// Gets the underlying Animator component for advanced usage.
        /// </summary>
        public Animator GetAnimator() => _animator;

        #endregion

        #region Private Methods

        private int GetParamHash(string paramName)
        {
            if (!_parameterHashMap.TryGetValue(paramName, out int hash))
            {
                hash = Animator.StringToHash(paramName);
                _parameterHashMap[paramName] = hash;
            }
            return hash;
        }

        #endregion
    }


    #region Usage Examples
    /*
    // EXAMPLE USAGE:
    
    // 1. Basic Setup:
    // Attach this script to a GameObject with an Animator component.
    // The required Animator component will be automatically added if missing.

    
    // 2. In your character controller or state machine:
    
    private AnimationController _animationController;
    
    private void Awake()
    {
        _animationController = GetComponent<AnimationController>();
    }
    
    private void Update()
    {
        // Play animations by name (automatically hashed)
        _animationController.Play("Run");
        
        // Set parameters by name (automatically hashed)
        _animationController.SetBool("IsGrounded", isGrounded);
        _animationController.SetFloat("MoveSpeed", currentSpeed);
        _animationController.SetTrigger("Attack");
        
        // For better performance, cache hashes and use the int overloads:
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
        
        // Then use the hashed versions (faster):
        _animationController.SetBool(IsGroundedHash, isGrounded);
        _animationController.SetFloat(MoveSpeedHash, currentSpeed);
        _animationController.SetTrigger(AttackTriggerHash);
    }
    
    // 3. For animation events that need to call back to the controller:
    // In the Animation window, add an AnimationEvent and set the function to a public method in your script.
    // The AnimationController will forward these events to the appropriate component.
    
    // Example animation event handler in your character controller:
    public void OnAttackAnimationEvent()
    {
        // Handle attack hit detection, sound effects, etc.
    }
    */
    #endregion
}
