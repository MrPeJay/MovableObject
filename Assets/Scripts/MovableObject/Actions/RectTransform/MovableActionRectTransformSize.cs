using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Actions
{
    public class MovableActionRectTransformSize : MovableActionRectTransform, IMovableActionCurrentState
    {
        [SerializeField] private Vector2 size;
        private Vector2 _previousSize;

        public MovableActionRectTransformSize()
        {
        }

        public MovableActionRectTransformSize(RectTransform rectTransform, CanvasScaler canvasScaler) : base(
            rectTransform, canvasScaler)
        {
        }

        protected override void ResetDefaultValues()
        {
            size = new Vector2();
        }

        public override Tween GetTween(float actionTime)
        {
            return RectTransform.DOSizeDelta(size, ActionTime(actionTime));
        }

        public override void SetInitialState()
        {
            RectTransform.sizeDelta = size;
        }

        public override void ResetPreviousState()
        {
            RectTransform.sizeDelta = _previousSize;
        }

        public override void SaveObjectValues()
        {
            _previousSize = RectTransform.sizeDelta;
        }

        public override ActionType Type()
        {
            return ActionType.Size;
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            size = RectTransform.sizeDelta;
        }
    }
}