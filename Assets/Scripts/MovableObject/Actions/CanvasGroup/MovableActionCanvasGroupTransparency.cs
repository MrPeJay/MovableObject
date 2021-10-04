using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionCanvasGroupTransparency : MovableActionCanvasGroup, IMovableTransparency,
        IMovableActionCurrentState
    {
        [OdinSerialize]
        [PropertyRange(0f, 1f)]
        public float Alpha { get; set; }

        public float PreviousAlpha { get; set; }

        public MovableActionCanvasGroupTransparency() { }

        public MovableActionCanvasGroupTransparency(CanvasGroup canvasGroup) : base(canvasGroup)
        {
        }

        protected override void ResetDefaultValues()
        {
            Alpha = 0f;
        }

        public override Tween GetTween(float actionTime)
        {
            return CanvasGroup.DOFade(Alpha, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            CanvasGroup.alpha = Alpha;
        }

        public override void ResetPreviousState()
        {
            CanvasGroup.alpha = PreviousAlpha;
        }

        public override void SaveObjectValues()
        {
            PreviousAlpha = CanvasGroup.alpha;
        }

        public override ActionType Type()
        {
            return ActionType.Transparency;
        }

        public override MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (actionToCopyFrom is IMovableTransparency actionToCopyTransparency)
                Alpha = actionToCopyTransparency.Alpha;

            return base.Copy(actionToCopyFrom);
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            Alpha = CanvasGroup.alpha;
        }
    }
}