using System;
using System.Collections;
using MovableObject.Actions;
using MovableObject.Extensions;
using MovableObject.Staging;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Scriptable
{
    [CreateAssetMenu(fileName = "Movable Action", menuName = "Movable Object/Movable Action")]
    public class ScriptableMovableAction : ScriptableMovableBase
    {
        [ShowIf("@Action != null")] [PropertySpace(20)] [OdinSerialize]
        public MovableAction Action;

        public override void AddAction()
        {
            var newAction = MovableStageBase
                .GetAction((MovableAction.ActionType) ActionType,
                    (MovableComponentExtension.ComponentType) ComponentType)
                .Copy(Action);

            Action = newAction;
        }

        /// <summary>
        /// Method invoked once action type is changed.
        /// Updates component type selection dropdown.
        /// </summary>
        protected override void OnActionTypeValueChanged()
        {
            if ((MovableAction.ActionType) ActionType == MovableAction.ActionType.None)
            {
                Action = null;
                return;
            }

            if (!(ComponentTypes() is ValueDropdownList<int> componentTypes))
                return;

            ComponentType = componentTypes[0].Value;

            AddAction();
        }

        /// <summary>
        /// Returns action type value dropdown list.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable ActionTypes()
        {
            var dropDownList = new ValueDropdownList<int>();

            var actionTypeNames = Enum.GetNames(typeof(MovableAction.ActionType));
            var actionCount = actionTypeNames.Length;

            for (var i = 0; i < actionCount; i++)
            {
                var currentActionType =
                    (MovableAction.ActionType) Enum.Parse(typeof(MovableAction.ActionType), actionTypeNames[i]);

                //Ignore scriptable action and scriptable action group types.
                if (currentActionType == MovableAction.ActionType.ScriptableAction)
                    continue;

                dropDownList.Add(currentActionType.ToString(), (int) currentActionType);
            }

            return dropDownList;
        }

        protected override bool ShowComponentTypeSelection()
        {
            return ActionType != (int) MovableAction.ActionType.None;
        }

        public override MovableAction[] GetActions()
        {
            return Action == null ? null : new MovableAction[] {(MovableAction) Action.Clone()};
        }

        public void SetActionType(MovableAction.ActionType actionType)
        {
            ActionType = (int) actionType;
        }

        public void SetComponentType(MovableComponentExtension.ComponentType componentType)
        {
            ComponentType = (int) componentType;
        }
    }
}