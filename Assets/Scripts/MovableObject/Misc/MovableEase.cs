using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject.Misc
{
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    [HideLabel]
    public class MovableEase : ICloneable
    {
        [EnumToggleButtons, HideLabel, GUIColor(0, 1f, .25f)]
        public MovableEaseType Type = MovableEaseType.DOTweenEase;

        [ShowIf("@Type == MovableEaseType.DOTweenEase")] [HideLabel]
        public DG.Tweening.Ease EaseType;

        [ShowIf("@Type == MovableEaseType.AnimationCurve")] [HideReferenceObjectPicker] [HideLabel]
        public AnimationCurve AnimationCurveEase = new AnimationCurve();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public enum MovableEaseType
    {
        DOTweenEase = 1 << 1,
        AnimationCurve = 1 << 2
    }
}