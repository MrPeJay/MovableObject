using MovableObject.Extensions;
using UnityEngine;

namespace MovableObject.Actions
{
    public abstract class MovableActionMeshRenderer : MovableAction, IMovableComponent
    {
        [SerializeField] [HideInInspector] protected MeshRenderer MeshRenderer;

        protected MovableActionMeshRenderer()
        {
        }

        protected MovableActionMeshRenderer(MeshRenderer meshRenderer)
        {
            MeshRenderer = meshRenderer;
        }

        public bool AssignComponents(Component[] components)
        {
            var component = components[0];

            if (component != null && component is MeshRenderer meshRenderer)
            {
                MeshRenderer = meshRenderer;
                return true;
            }

            Debug.LogError(Warning<MeshRenderer>(component));
            return false;
        }

        public bool ContainsComponents()
        {
            return MeshRenderer != null;
        }

        public override MovableComponentExtension.ComponentType GetComponentType()
        {
            return MovableComponentExtension.ComponentType.MeshRenderer;
        }
    }
}