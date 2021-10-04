using System;
using System.Collections;
using System.Collections.Generic;
using MovableObject.Actions;
using MovableObject.Extensions;
using MovableObject.Staging;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Scriptable
{
    [CreateAssetMenu(fileName = "Movable Action Group", menuName = "Movable Object/Movable Action Group")]
    public class ScriptableMovableActionGroup : ScriptableMovableBase
    {
        [BoxGroup(ActionSelectionGroup)]
        [HideIf("@ActionType != (int) MovableAction.ActionType.ScriptableAction")]
        [OnValueChanged("AddScriptableAction")]
        [AssetSelector]
        [ShowInInspector]
        private ScriptableMovableBase scriptableMovableAction;

        [PropertySpace(20)] [OdinSerialize] [ListDrawerSettings(HideAddButton = true, NumberOfItemsPerPage = 3)]
        public List<MovableAction> Actions = new List<MovableAction>();

        private ScriptableMovableBase _prevAddedScriptableMovableAction;

        public override void AddAction()
        {
            //Ignore if None selected.
            if (ComponentType == 0)
                return;

            var newAction = MovableStageBase
                .GetAction((MovableAction.ActionType) ActionType,
                    (MovableComponentExtension.ComponentType) ComponentType);

            newAction.ToggleSaveButtonVisibility(true);

            Actions.Add(newAction);

            ActionType = 0;
        }

        protected override void OnActionTypeValueChanged()
        {
            if ((MovableAction.ActionType) ActionType == MovableAction.ActionType.None)
                return;

            if (!(ComponentTypes() is ValueDropdownList<int> componentTypes))
                return;

            ComponentType = componentTypes[0].Value;

            AddAction();
        }

        protected override IEnumerable ActionTypes()
        {
            var dropDownList = new ValueDropdownList<int>();

            var actionTypeNames = Enum.GetNames(typeof(MovableAction.ActionType));
            var actionCount = actionTypeNames.Length;

            for (var i = 0; i < actionCount; i++)
            {
                var currentActionType =
                    (MovableAction.ActionType) Enum.Parse(typeof(MovableAction.ActionType), actionTypeNames[i]);

                dropDownList.Add(currentActionType.ToString(), (int) currentActionType);
            }

            return dropDownList;
        }

        protected override IEnumerable ComponentTypes()
        {
            //Add none value.
            var valueDropdownList = new ValueDropdownList<int>
            {
                new ValueDropdownItem<int>(MovableComponentExtension.ComponentType.None.ToString(),
                    (int) MovableComponentExtension.ComponentType.None)
            };

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
        /// Adds a scriptable action to the end of list by creating a copy of the original action.
        /// </summary>
        private void AddScriptableAction()
        {
            if (_prevAddedScriptableMovableAction != null || scriptableMovableAction == null)
            {
                _prevAddedScriptableMovableAction = null;
                scriptableMovableAction = null;
                return;
            }

            var assignedScriptableAction = scriptableMovableAction;
            scriptableMovableAction = null;

            var scriptableActions = assignedScriptableAction.GetActions();

            if (scriptableActions == null)
            {
                Debug.LogError(
                    "Scriptable action doesn't contain any actions. " +
                    "Please check the scriptable action and make sure actions are assigned");
                return;
            }

            var count = scriptableActions.Length;

            for (var i = 0; i < count; i++)
            {
                var action = scriptableActions[i];

                if (action == null)
                {
                    Debug.LogError(
                        "Scriptable action doesn't have any actions specified." +
                        " Action is null. Assign an action and component types and try again.");
                    return;
                }

                action.ToggleSaveButtonVisibility(true);

                MovableStageBase.AddAction(action, ref Actions);
            }

            ActionType = (int) MovableAction.ActionType.None;

            _prevAddedScriptableMovableAction = assignedScriptableAction;
        }

        protected override bool ShowComponentTypeSelection()
        {
            return ActionType != (int) MovableAction.ActionType.None &&
                   ActionType != (int) MovableAction.ActionType.ScriptableAction;
        }

        public override MovableAction[] GetActions()
        {
            var clonedActionList = new List<MovableAction>();

            var actionCount = Actions.Count;

            for (var i = 0; i < actionCount; i++)
                clonedActionList.Add((MovableAction) Actions[i].Clone());

            return clonedActionList.ToArray();
        }
    }
}