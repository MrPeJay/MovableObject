using System;
using System.Collections;
using MovableObject.Actions;
using MovableObject.Extensions;
using MovableObject.Staging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject.Scriptable
{
    public abstract class ScriptableMovableBase : SerializedScriptableObject, IMovableStage
    {
        protected const string ActionSelectionGroup = "Select Action";

        [BoxGroup(ActionSelectionGroup)]
        [SerializeField]
        [OnValueChanged("OnActionTypeValueChanged")]
        [ValueDropdown("ActionTypes", ExpandAllMenuItems = true)]
        protected int ActionType;

        [ShowIf("ShowComponentTypeSelection")]
        [BoxGroup(ActionSelectionGroup)]
        [ValueDropdown("ComponentTypes", ExpandAllMenuItems = true)]
        [OnValueChanged("AddAction")]
        [SerializeField]
        protected int ComponentType;

        public abstract void AddAction();

        /// <summary>
        /// Method invoked once action type is changed.
        /// Updates component type selection dropdown.
        /// </summary>
        protected abstract void OnActionTypeValueChanged();

        /// <summary>
        /// Returns action type value dropdown list.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable ActionTypes();

        /// <summary>
        /// Returns component type value dropdown list based on the selected action type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable ComponentTypes()
        {
            var valueDropdownList = new ValueDropdownList<int>();

            var types = MovableComponentExtension.GetIMovableActionTypes();

            foreach (var type in types)
            {
                //Create an instant with a default constructor.
                var instance = (IMovableActionComponent) Activator.CreateInstance(type);

                var instanceActionType = instance.Type();

                //Check if the action type matches the specified one in the editor.
                if (instanceActionType != (MovableAction.ActionType) ActionType) continue;

                var instanceComponentType = instance.GetComponentType();

                var valueItem = new ValueDropdownItem<int>()
                    {Text = instanceComponentType.ToString(), Value = (int) instanceComponentType};

                //If value doesn't already exist in the list, add it.
                if (!valueDropdownList.Contains(valueItem))
                    valueDropdownList.Add(valueItem);
            }

            return valueDropdownList;
        }

        /// <summary>
        /// Returns whether to show component type selection in the editor.
        /// </summary>
        /// <returns></returns>
        protected abstract bool ShowComponentTypeSelection();

        /// <summary>
        /// Returns scriptable action array.
        /// </summary>
        /// <returns></returns>
        public abstract MovableAction[] GetActions();
    }
}