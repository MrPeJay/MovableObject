using System.Collections.Generic;
using DG.Tweening;
using MovableObject.Actions;
using MovableObject.Staging;
using UnityEngine;

namespace MovableObject
{
    public class MovableObjectMultiStage : MovableObjectBase
    {
        [SerializeField] protected virtual MovableStage[] Stages { get; set; }

        protected int _currentStageIndex;

        public override void SetInitialState()
        {
            Stages[0].SetInitialState();

            _currentStageIndex = 0;
        }

        public override void ResetPreviousState()
        {
            //This probably ain't the most efficient way of checking unique types existing in the list.
            //But it works ey :)

            var stageCount = Stages.Length;
            var actionList = new List<MovableAction>();

            for (var i = 0; i < stageCount; i++)
            {
                var actions = Stages[i].GetActions();
                var actionCount = actions.Length;

                //Go through all the actions and check whether the action type already exists in the list.
                for (var j = 0; j < actionCount; j++)
                {
                    var currentAction = actions[j];

                    var actionType = currentAction.Type();
                    var isFound = false;

                    var assignedActionCount = actionList.Count;

                    for (var k = 0; k < assignedActionCount; k++)
                    {
                        if (actionList[k].Type() != actionType) continue;

                        isFound = true;
                        break;
                    }

                    if (isFound) continue;

                    actionList.Add(currentAction);
                    currentAction.ResetPreviousState();
                }
            }
        }

        public override void SaveObjectValues()
        {
            var stageCount = Stages.Length;

            for (var i = 0; i < stageCount; i++)
                Stages[i].SaveObjectValues();
        }

        public override int CurrentStageIndex()
        {
            return _currentStageIndex;
        }

        public override Tween PlayAction()
        {
            if (Stages.Length > _currentStageIndex + 1)
                _currentStageIndex++;
            else
                _currentStageIndex = 0;

            var action = Stages[_currentStageIndex].PlayAction(ActionTime()).SetUpdate(UseUnscaledTime);

            return action;
        }
    }
}