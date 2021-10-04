using System;
using System.Collections.Generic;
using DG.Tweening;
using MovableObject.Actions;
using MovableObject.Misc;
using MovableObject.Scriptable;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Staging
{
    [Serializable]
    [InlineProperty]
    [HideLabel]
    [HideReferenceObjectPicker]
    public class MovableStage : IMovableTween, IMovableStage
    {
        private const string StageGroup = "Stage";

        public Tween CurrentTween { get; set; }

        [FoldoutGroup(StageGroup)]
        [SerializeField]
        private MovableDelay delay = new MovableDelay();

        [FoldoutGroup(StageGroup)] [OnValueChanged("AddAction")] [SerializeField]
        private MovableAction.ActionType addActionType;

        [FoldoutGroup(StageGroup)]
        [ShowIf("ShowScriptableMovableActionSelection")]
        [OnValueChanged("AddScriptableAction")]
        [AssetSelector] 
        [ShowInInspector]
        private ScriptableMovableBase scriptableMovableAction;

        [OdinSerialize]
        [FoldoutGroup(StageGroup)]
        [ListDrawerSettings(HideAddButton = true, NumberOfItemsPerPage = 1)]
        private List<MovableAction> actions = new List<MovableAction>();

        [FoldoutGroup(StageGroup)] [SerializeField]
        private MovableEventsHolder eventHolder = new MovableEventsHolder();

        [SerializeField] [HideInInspector] private MovableObjectBase _movableObject;
        private MovableAction.ActionType _prevActionType;

        private ScriptableMovableBase _prevAddedScriptableMovableAction;

        public MovableStage(MovableObjectBase movableObject)
        {
            _movableObject = movableObject;
        }

        /// <summary>
        /// Returns stage's currently assigned actions.
        /// </summary>
        /// <returns></returns>
        public MovableAction[] GetActions()
        {
            return actions.ToArray();
        }

        /// <summary>
        /// Returns whether to show scriptable movable action selection field in the editor.
        /// </summary>
        /// <returns></returns>
        private bool ShowScriptableMovableActionSelection()
        {
            return addActionType == MovableAction.ActionType.ScriptableAction;
        }

        /// <summary>
        /// Returns the amount of time for all actions to be completed.
        /// </summary>
        /// <param name="movableObject"></param>
        /// <returns></returns>
        private float ActionTime(float actionTime)
        {
            var count = actions.Count;
            var time = 0f;

            for (var i = 0; i < count; i++)
            {
                var currentActionTime = actions[i].ActionTime(actionTime);

                if (currentActionTime > time)
                    time = currentActionTime;
            }

            return time;
        }

        /// <summary>
        /// Plays all stage action simultaneously.
        /// </summary>
        /// <returns></returns>
        public Tween PlayAction(float actionTime)
        {
            eventHolder.StartEvent();

            var count = actions.Count;
            var mySequence = DOTween.Sequence();

            for (var i = 0; i < count; i++)
                mySequence.Join(actions[i].PlayAction(ActionTime(actionTime)));

            CurrentTween = delay.UseDelay
                ? mySequence.OnComplete(eventHolder.StartEvent).SetDelay(delay.DelayTime)
                : mySequence.OnComplete(eventHolder.StartEvent);

            return CurrentTween;
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

                if (!(action is IMovableComponent movableComponent))
                {
                    Debug.LogError("Something went wrong, maybe try again.");
                    return;
                }

                if (!movableComponent.AssignComponents(
                    MovableStageBase.GetActionComponents(_movableObject, action.GetComponentType())))
                    return;

                action.ToggleSaveButtonVisibility(true);
                MovableStageBase.AddAction(action, ref actions);
            }

            addActionType = MovableAction.ActionType.None;
            _prevActionType = MovableAction.ActionType.ScriptableAction;

            _prevAddedScriptableMovableAction = assignedScriptableAction;
        }

        public void AddAction()
        {
            //Prevent duplicates with a single add.
            if (_prevActionType == addActionType)
            {
                addActionType = MovableAction.ActionType.None;
                _prevActionType = MovableAction.ActionType.None;
                return;
            }

            switch (addActionType)
            {
                case MovableAction.ActionType.ScriptableAction:
                    scriptableMovableAction = null;
                    return;
            }

            var action = MovableStageBase.GetAction(_movableObject, addActionType);
            action.ToggleSaveButtonVisibility(true);

            MovableStageBase.AddAction(action, ref actions);

            _prevActionType = addActionType;
            addActionType = MovableAction.ActionType.None;
        }

        public void Stop(bool complete = false)
        {
            CurrentTween?.Kill(complete);
        }

        public void SetInitialState()
        {
            var actionCount = actions.Count;

            for (var i = 0; i < actionCount; i++)
                actions[i].SetInitialState();
        }

        public void ResetPreviousState()
        {
            var actionCount = actions.Count;

            for (var i = 0; i < actionCount; i++)
                actions[i].ResetPreviousState();
        }

        public void SaveObjectValues()
        {
            var actionCount = actions.Count;

            for (var i = 0; i < actionCount; i++)
                actions[i].SaveObjectValues();
        }
    }
}