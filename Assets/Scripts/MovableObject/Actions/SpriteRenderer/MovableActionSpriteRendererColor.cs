using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionSpriteRendererColor : MovableActionSpriteRenderer, IMovableActionCurrentState,
        IMovableColor
    {
        [OdinSerialize] public Color Color { get; set; }
        public Color PreviousColor { get; set; }

        public MovableActionSpriteRendererColor()
        {
        }

        public MovableActionSpriteRendererColor(SpriteRenderer spriteRenderer) : base(spriteRenderer)
        {
        }

        protected override void ResetDefaultValues()
        {
            Color = new Color();
        }

        public override Tween GetTween(float actionTime)
        {
            return SpriteRenderer.DOColor(Color, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            SpriteRenderer.color = Color;
        }

        public override void ResetPreviousState()
        {
            SpriteRenderer.color = PreviousColor;
        }

        public override void SaveObjectValues()
        {
            PreviousColor = SpriteRenderer.color;
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
            Color = SpriteRenderer.color;
        }
    }
}