using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionTransformPosition : MovableActionTransform, IMovableActionCurrentState
    {
        [SerializeField] private bool localPosition;
        [SerializeField] private Vector3 position;

        private Vector3 _previousPosition;

        public MovableActionTransformPosition()
        {
        }

        public MovableActionTransformPosition(Transform transform) : base(transform)
        {
        }

        protected override void ResetDefaultValues()
        {
            localPosition = false;
            position = new Vector3();
        }

        public override Tween GetTween(float actionTime)
        {
            return localPosition
                ? Transform.DOLocalMove(position, actionTime)
                : Transform.DOMove(position, actionTime);
        }

        public override void SetInitialState()
        {
            if (localPosition)
                Transform.localPosition = position;
            else
                Transform.position = position;
        }

        public override void ResetPreviousState()
        {
            Transform.position = _previousPosition;
        }

        public override void SaveObjectValues()
        {
            _previousPosition = Transform.position;
        }

        public override ActionType Type()
        {
            return ActionType.Position;
        }

        public override MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (!(actionToCopyFrom is MovableActionTransformPosition actionTransformPosition))
                return base.Copy(actionToCopyFrom);

            position = actionTransformPosition.position;
            localPosition = actionTransformPosition.localPosition;

            return base.Copy(actionToCopyFrom);
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            position = localPosition ? Transform.localPosition : Transform.position;
        }
    }
}