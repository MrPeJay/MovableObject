using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionTransformRotation : MovableActionTransform, IMovableActionCurrentState
    {
        [SerializeField] private bool useQuaternion;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Quaternion quaternion;

        private Quaternion _previousQuaternion;

        public MovableActionTransformRotation()
        {
        }

        public MovableActionTransformRotation(Transform transform) : base(transform)
        {
        }

        protected override void ResetDefaultValues()
        {
            useQuaternion = false;
            rotation = new Vector3();
            quaternion = new Quaternion();
        }

        public override Tween GetTween(float actionTime)
        {
            return Transform.DORotate(rotation, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            if (useQuaternion)
            {
                Transform.rotation = quaternion;
                return;
            }

            Transform.rotation = Quaternion.Euler(rotation);
        }

        public override void ResetPreviousState()
        {
            Transform.rotation = _previousQuaternion;
        }

        public override void SaveObjectValues()
        {
            _previousQuaternion = Transform.rotation;
        }

        public override ActionType Type()
        {
            return ActionType.Rotation;
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            rotation = Transform.rotation.eulerAngles;
        }
    }
}