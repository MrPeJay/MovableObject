using System;
using DG.Tweening;
using MovableObject.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Actions
{
    public class MovableActionRectTransformPosition : MovableActionRectTransform, IMovableActionCurrentState
    {
        [SerializeField] private bool useCustomPosition;

        [ShowIf("useCustomPosition")] [SerializeField]
        private PositionType positionType;

        [ShowIf("@useCustomPosition && positionType == PositionType.WorldPosition")] [SerializeField]
        private Vector3 worldPosition;

        [ShowIf("@useCustomPosition && positionType == PositionType.LocalPosition")][SerializeField]
        private Vector3 localPosition;

        [ShowIf("@useCustomPosition && positionType == PositionType.AnchoredPosition")] [SerializeField]
        private Vector2 anchoredPosition;

        [ShowIf("@!useCustomPosition")] [SerializeField]
        private ScreenPosition position;

        private Vector3 _previousPosition;

        private enum PositionType
        {
            WorldPosition,
            LocalPosition,
            AnchoredPosition
        }

        public MovableActionRectTransformPosition()
        {
        }

        public MovableActionRectTransformPosition(RectTransform rectTransform, CanvasScaler canvasScaler) : base(
            rectTransform, canvasScaler)
        {
        }

        protected override void ResetDefaultValues()
        {
            worldPosition = new Vector3();
            anchoredPosition = new Vector2();
            useCustomPosition = false;
            positionType = PositionType.AnchoredPosition;
            position = (ScreenPosition) 0;
        }

        public override Tween GetTween(float actionTime)
        {
            if (!useCustomPosition)
                return RectTransform.DOAnchorPos(RectTransform.GetScreenPosition(position, CanvasScaler), actionTime);

            switch (positionType)
            {
	            case PositionType.WorldPosition:
		            return RectTransform.DOMove(worldPosition, actionTime);
	            case PositionType.LocalPosition:
		            return RectTransform.DOLocalMove(localPosition, actionTime);
                case PositionType.AnchoredPosition:
		            return RectTransform.DOAnchorPos(anchoredPosition, actionTime);
	            default:
		            throw new ArgumentOutOfRangeException();
            }

        }

        public override void SetInitialState()
        {
            if (!useCustomPosition)
            {
                RectTransform.anchoredPosition = RectTransform.GetScreenPosition(position, CanvasScaler);
                return;
            }

            switch (positionType)
            {
	            case PositionType.WorldPosition:
		            RectTransform.position = worldPosition;
                    break;
	            case PositionType.LocalPosition:
		            RectTransform.localPosition = localPosition;
		            break;
	            case PositionType.AnchoredPosition:
		            RectTransform.anchoredPosition = anchoredPosition;
                    break;
	            default:
		            throw new ArgumentOutOfRangeException();
            }
        }

        public override void ResetPreviousState()
        {
            RectTransform.position = _previousPosition;
        }

        public override void SaveObjectValues()
        {
            _previousPosition = RectTransform.position;
        }

        public override ActionType Type()
        {
            return ActionType.Position;
        }

        public override MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (!(actionToCopyFrom is MovableActionRectTransformPosition actionRectTransformPosition))
                return base.Copy(actionToCopyFrom);

            useCustomPosition = actionRectTransformPosition.useCustomPosition;
            positionType = actionRectTransformPosition.positionType;
            worldPosition = actionRectTransformPosition.worldPosition;
            anchoredPosition = actionRectTransformPosition.anchoredPosition;
            position = actionRectTransformPosition.position;

            return base.Copy(actionToCopyFrom);
        }

        [ShowIf("@useCustomPosition && ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
	        switch (positionType)
	        {
		        case PositionType.WorldPosition:
			        worldPosition = RectTransform.position;
			        break;
		        case PositionType.LocalPosition:
			        localPosition = RectTransform.localPosition;
			        break;
		        case PositionType.AnchoredPosition:
			        anchoredPosition = RectTransform.anchoredPosition;
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }
    }
}