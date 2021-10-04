using System;
using Sirenix.OdinInspector;

namespace MovableObject.Misc
{
    [Serializable]
    [HideLabel]
    [InlineProperty]
    [HideReferenceObjectPicker]
    public class MovableDelay : ICloneable
    {
        [ToggleGroup("UseDelay", "Delay")] public bool UseDelay;

        [ToggleGroup("UseDelay", "Delay")] public float DelayTime;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}