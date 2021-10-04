using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;

namespace MovableObject.Actions
{
    public class MovableActionImageTransparency : MovableActionImage, IMovableActionCurrentState, IMovableTransparency
    {
        [OdinSerialize] [PropertyRange(0f, 1f)] public float Alpha { get; set; }
        public float PreviousAlpha { get; set; }

        public MovableActionImageTransparency()
        {

        }

        public MovableActionImageTransparency(Image image) : base(image)
        {
        }

        protected override void ResetDefaultValues()
        {
            Alpha = 0f;
        }

        public override Tween GetTween(float actionTime)
        {
            return Image.DOFade(Alpha, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            var currentColor = Image.color;
            PreviousAlpha = currentColor.a;

            currentColor.a = Alpha;

            Image.color = currentColor;
        }

        public override void ResetPreviousState()
        {
            var currentColor = Image.color;
            currentColor.a = PreviousAlpha;

            Image.color = currentColor;
        }

        public override void SaveObjectValues()
        {
            var currentColor = Image.color;
            PreviousAlpha = currentColor.a;
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
            Alpha = Image.color.a;
        }
    }
}