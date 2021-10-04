using MovableObject.Extensions;
using UnityEngine;

namespace MovableObject.Actions
{
    public abstract class MovableActionTransform : MovableAction, IMovableComponent
    {
        [SerializeField] [HideInInspector] protected Transform Transform;

        protected MovableActionTransform()
        {
        }

        protected MovableActionTransform(Transform transform)
        {
            Transform = transform;
        }

        public bool AssignComponents(Component[] components)
        {
            var component = components[0];

            if (component is Transform transform)
            {
                Transform = transform;
                return true;
            }

            Debug.LogError(Warning<Transform>(component));
            return false;
        }

        public bool ContainsComponents()
        {
            return Transform != null;
        }

        public override MovableComponentExtension.ComponentType GetComponentType()
        {
            return MovableComponentExtension.ComponentType.Transform;
        }
    }
}