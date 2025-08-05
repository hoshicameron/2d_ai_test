using UnityEngine;

namespace PetalsOfHope.UI.Base
{
    /// <summary>
    /// Abstract base class for all UI views (Screens and Panels).
    /// Provides common functionality for showing, hiding, and managing the view's lifecycle.
    /// </summary>
    public abstract class UIView : MonoBehaviour
    {
        /// <summary>
        /// Called to initialize the view.
        /// </summary>
        public virtual void Initialize()
        {
            // Base implementation can be empty.
        }

        /// <summary>
        /// Called to clean up the view before it's destroyed.
        /// </summary>
        public virtual void Terminate()
        {
            // Base implementation can be empty.
        }

        /// <summary>
        /// Activates the view's GameObject, making it visible.
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivates the view's GameObject, hiding it.
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
