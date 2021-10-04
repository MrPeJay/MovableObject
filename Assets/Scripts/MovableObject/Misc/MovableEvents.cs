using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace MovableObject.Misc
{
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public struct MovableEvents
    {
        private const string StartTabGroup = "Start Events", EndTabGroup = "End Events";

        [ToggleGroup("UseEvents")] public bool UseEvents;

        [TabGroup("UseEvents/Tab Group", StartTabGroup)] [DrawWithUnity]
        public UnityEvent OnStart;

        [TabGroup("UseEvents/Tab Group", EndTabGroup)] [DrawWithUnity]
        public UnityEvent OnEnd;
    }

    [Serializable]
    [InlineProperty]
    [HideLabel]
    [HideReferenceObjectPicker]
    public class MovableEventsHolder : ICloneable
    {
        public MovableEvents Events;

        /// <summary>
        /// Invokes start event if specified.
        /// </summary>
        public void StartEvent()
        {
            if (Events.UseEvents)
                Events.OnStart?.Invoke();
        }

        /// <summary>
        /// Invokes end event if specified.
        /// </summary>
        public void EndEvent()
        {
            if (Events.UseEvents)
                Events.OnEnd?.Invoke();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}