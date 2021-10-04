using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Actions
{
    public class MovableActionImageColor : MovableActionImage, IMovableActionCurrentState, IMovableColor
    {
        [OdinSerialize] public Color Color { get; set; }
        public Color PreviousColor { get; set; }

        public MovableActionImageColor()
        {
        }

        public MovableActionImageColor(Image image) : base(image)
        {
        }

        protected override void ResetDefaultValues()
        {
            Color = new Color();
        }

        public override Tween GetTween(float actionTime)
        {
            return Image.DOColor(Color, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            Image.color = Color;
        }

        public override void ResetPreviousState()
        {
            Image.color = PreviousColor;
        }

        public override void SaveObjectValues()
        {
            PreviousColor = Image.color;
        }

        public override ActionType Type()
        {
            return ActionType.Color;
        }

        public override MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (actionToCopyFrom is IMovableColor colorAction)
                Color = colorAction.Color;

            return base.Copy(actionToCopyFrom);
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            Color = Image.color;
        }
    }
}