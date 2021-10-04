using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionSpriteRendererTransparency : MovableActionSpriteRenderer, IMovableActionCurrentState,
        IMovableTransparency
    {
        [OdinSerialize]
        [PropertyRange(0f, 1f)]
        public float Alpha { get; set; }

        public float PreviousAlpha { get; set; }

        public MovableActionSpriteRendererTransparency()
        {
        }

        public MovableActionSpriteRendererTransparency(SpriteRenderer spriteRenderer) : base(spriteRenderer)
        {
        }

        protected override void ResetDefaultValues()
        {
            Alpha = 0f;
        }

        public override Tween GetTween(float actionTime)
        {
            return SpriteRenderer.DOFade(Alpha, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            var currentColor = SpriteRenderer.color;
            currentColor.a = Alpha;

            SpriteRenderer.color = currentColor;
        }

        public override void ResetPreviousState()
        {
            var currentColor = SpriteRenderer.color;
            currentColor.a = PreviousAlpha;

            SpriteRenderer.color = currentColor;
        }

        public override void SaveObjectValues()
        {
            PreviousAlpha = SpriteRenderer.color.a;
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
            Alpha = SpriteRenderer.color.a;
        }
    }
}