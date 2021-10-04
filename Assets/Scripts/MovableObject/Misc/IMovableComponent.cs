using UnityEngine;

namespace MovableObject.Actions
{
    public interface IMovableComponent
    {
        /// <summary>
        /// Assigns action necessary components to function properly.
        /// </summary>
        /// <param name="components"></param>
        /// /// <returns>If all components assigned.</returns>
        bool AssignComponents(Component[] components);

        /// <summary>
        /// Returns whether the movable action contains all the necessary components.
        /// </summary>
        /// <returns></returns>
        bool ContainsComponents();
    }
}