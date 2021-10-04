using DG.Tweening;

namespace MovableObject
{
    public interface IMovableTween
    {
        Tween CurrentTween { get; set; }

        /// <summary>
        /// Stops current tween if available.
        /// </summary>
        void Stop(bool complete = false);

        /// <summary>
        /// Sets the initial state of the object based on the specified action values.
        /// </summary>
        void SetInitialState();

        /// <summary>
        /// Resets object state to previous state.
        /// Used when stopping the preview.
        /// </summary>
        void ResetPreviousState();

        /// <summary>
        /// Saves initial values before starting preview mode.
        /// Used to reset object values when stopping preview mode.
        /// </summary>
        void SaveObjectValues();
    }
}