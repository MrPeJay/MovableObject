using MovableObject.Extensions;
using UnityEngine;

namespace MovableObject.Actions
{
    public abstract class MovableActionCanvasGroup : MovableAction, IMovableComponent
    {
        [SerializeField] [HideInInspector] protected CanvasGroup CanvasGroup;

        protected MovableActionCanvasGroup() { }

        protected MovableActionCanvasGroup(CanvasGroup canvasGroup)
        {
            CanvasGroup = canvasGroup;
        }

        public bool AssignComponents(Component[] components)
        {
            var component = components[0];

            if (component is CanvasGroup canvasGroup)
            {
                CanvasGroup = canvasGroup;
                return true;
            }

            Debug.LogError(Warning<CanvasGroup>(component));
            return false;
        }

        public bool ContainsComponents()
        {
            return CanvasGroup != null;
        }

        public override MovableComponentExtension.ComponentType GetComponentType()
        {
            return MovableComponentExtension.ComponentType.CanvasGroup;
        }
    }
}