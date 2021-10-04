using MovableObject.Actions;
using MovableObject.Extensions;

namespace MovableObject
{
    public interface IMovableActionComponent
    {
        /// <summary>
        /// Returns the type of the action.
        /// </summary>
        /// <returns></returns>
        MovableAction.ActionType Type();

        /// <summary>
        /// Returns the component type of the action.
        /// </summary>
        /// <returns></returns>
        MovableComponentExtension.ComponentType GetComponentType();
    }
}