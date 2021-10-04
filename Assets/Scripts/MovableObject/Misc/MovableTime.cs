using System;
using Sirenix.OdinInspector;

namespace MovableObject.Misc
{
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public struct MovableTime : ICloneable
    {
        private const string TimeGroup = "Time";

        [ToggleGroup("OverrideTime")] public bool OverrideTime;

        [ToggleGroup("OverrideTime")] public float ActionTime;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}