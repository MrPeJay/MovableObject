using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionTransformScale : MovableActionTransform, IMovableActionCurrentState
    {
        [SerializeField] private Vector3 scale;

        private Vector3 _previousScale;

        public MovableActionTransformScale()
        {
        }

        public MovableActionTransformScale(Transform transform) : base(transform)
        {
        }

        protected override void ResetDefaultValues()
        {
            scale = new Vector3();
        }

        public override Tween GetTween(float actionTime)
        {
            return Transform.DOScale(scale, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            Transform.localScale = scale;
        }

        public override void ResetPreviousState()
        {
            Transform.localScale = _previousScale;
        }

        public override void SaveObjectValues()
        {
            _previousScale = Transform.localScale;
        }

        public override ActionType Type()
        {
            return ActionType.Scale;
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            scale = Transform.localScale;
        }
    }
}