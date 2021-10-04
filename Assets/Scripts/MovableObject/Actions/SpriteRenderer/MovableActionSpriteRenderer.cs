using MovableObject.Extensions;
using UnityEngine;

namespace MovableObject.Actions
{
    public abstract class MovableActionSpriteRenderer : MovableAction, IMovableComponent
    {
        [SerializeField] [HideInInspector] protected SpriteRenderer SpriteRenderer;

        protected MovableActionSpriteRenderer()
        {
        }

        protected MovableActionSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            SpriteRenderer = spriteRenderer;
        }

        public bool AssignComponents(Component[] components)
        {
            var component = components[0];

            if (component != null && component is SpriteRenderer spriteRenderer)
            {
                SpriteRenderer = spriteRenderer;
                return true;
            }

            Debug.LogError(Warning<SpriteRenderer>(component));
            return false;
        }

        public bool ContainsComponents()
        {
            return SpriteRenderer != null;
        }

        public override MovableComponentExtension.ComponentType GetComponentType()
        {
            return MovableComponentExtension.ComponentType.SpriteRenderer;
        }
    }
}