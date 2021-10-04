using MovableObject.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MovableObject.Actions
{
    public abstract class MovableActionImage : MovableAction, IMovableComponent
    {
        [SerializeField] [HideInInspector] protected Image Image;

        protected MovableActionImage()
        {
        }

        protected MovableActionImage(Image image)
        {
            Image = image;
        }

        public bool AssignComponents(Component[] components)
        {
            var component = components[0];

            if (component is Image image)
            {
                Image = image;
                return true;
            }

            Debug.LogError(Warning<Image>(component));
            return false;
        }

        public bool ContainsComponents()
        {
            return Image != null;
        }

        public override MovableComponentExtension.ComponentType GetComponentType()
        {
            return MovableComponentExtension.ComponentType.Image;
        }
    }
}