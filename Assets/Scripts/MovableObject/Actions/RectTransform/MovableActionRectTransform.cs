using MovableObject.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Actions
{
    public abstract class MovableActionRectTransform : MovableAction, IMovableComponent
    {
        protected MovableActionRectTransform() { }

        protected MovableActionRectTransform(RectTransform rectTransform, CanvasScaler canvasScaler)
        {
            RectTransform = rectTransform;
            CanvasScaler = canvasScaler;
        }

        [SerializeField] [HideInInspector] protected RectTransform RectTransform;
        [SerializeField] [HideInInspector] protected CanvasScaler CanvasScaler;

        public bool AssignComponents(Component[] components)
        {
            var rectComponent = components[0];
            var scalerComponent = components[1];

            if (rectComponent is RectTransform rectTransform)
                RectTransform = rectTransform;
            else
            {
                Debug.LogError(Warning<RectTransform>(rectComponent));
                return false;
            }

            if (scalerComponent is CanvasScaler canvasScaler)
                CanvasScaler = canvasScaler;
            else
            {
                Debug.LogError(Warning<CanvasScaler>(scalerComponent));
                return false;
            }

            return true;
        }

        public bool ContainsComponents()
        {
            return RectTransform != null && CanvasScaler != null;
        }

        public override MovableComponentExtension.ComponentType GetComponentType()
        {
            return MovableComponentExtension.ComponentType.RectTransform;
        }
    }
}