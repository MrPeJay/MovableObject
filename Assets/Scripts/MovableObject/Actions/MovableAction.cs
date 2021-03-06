using System;
using DG.Tweening;
using MovableObject.Extensions;
using MovableObject.Misc;
using MovableObject.Scriptable;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MovableObject.Actions
{
    [Serializable]
    [InlineProperty]
    [HideReferenceObjectPicker]
    [HideLabel]
    public abstract class MovableAction : MovableSaveable, IMovableTween, ICloneable, IMovableActionComponent
    {
        public Tween CurrentTween { get; set; }
        [Title("$Title")] [SerializeField] private MovableTime time;
        [SerializeField] private MovableDelay delay = new MovableDelay();
        [SerializeField] private MovableEase ease = new MovableEase();
        [SerializeField] [PropertyOrder(2)] private MovableEventsHolder eventHolder = new MovableEventsHolder();

        /// <summary>
        /// Returns the title of the action which is displayed in the editor.
        /// </summary>
        /// <returns></returns>
        public string Title()
        {
            return $"{Type()} Action ({GetComponentType()})";
        }

#if UNITY_EDITOR

        protected override string SaveAsScriptableObject()
        {
            var relativePath = base.SaveAsScriptableObject();

            if (string.IsNullOrEmpty(relativePath))
	            return null;

            var asset = ScriptableObject.CreateInstance<ScriptableMovableAction>();

            var thisAction = (MovableAction) Clone();

            asset.SetActionType(thisAction.Type());
            asset.SetComponentType(thisAction.GetComponentType());

            asset.Action = thisAction;
            asset.Action.ToggleSaveButtonVisibility(false);

            AssetDatabase.CreateAsset(asset, System.IO.Path.Combine("Assets", relativePath));
            AssetDatabase.SaveAssets();

            Debug.Log(
                $"Saved action as scriptable object in a specified path: {relativePath}");

            return relativePath;
        }

        /// <summary>
        /// Resets action to it's default values.
        /// </summary>
        [HorizontalGroup("HorizontalGroup")]
        [PropertyOrder(-1)]
        [Button(Name = "Reset"), GUIColor(1, 0.2f, 0)]
        private void ResetDefaultParameters()
        {
            time = new MovableTime();
            delay = new MovableDelay();
            ease = new MovableEase();
            eventHolder = new MovableEventsHolder();

            ResetDefaultValues();
        }

#endif

        /// <summary>
        /// Resets all customizable value to their default ones.
        /// </summary>
        protected abstract void ResetDefaultValues();

        /// <summary>
        /// Plays tween animation with the specified values.
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        public Tween PlayAction(float actionTime)
        {
            eventHolder.StartEvent();

            switch (ease.Type)
            {
                case MovableEaseType.DOTweenEase:
                    CurrentTween = delay.UseDelay
                        ? GetTween(ActionTime(actionTime)).SetEase(ease.EaseType)
                            .OnComplete(eventHolder.EndEvent).SetDelay(delay.DelayTime)
                        : GetTween(ActionTime(actionTime)).SetEase(ease.EaseType)
                            .OnComplete(eventHolder.EndEvent);
                    break;
                case MovableEaseType.AnimationCurve:
                    CurrentTween = delay.UseDelay
                        ? GetTween(ActionTime(actionTime)).SetEase(ease.AnimationCurveEase)
                            .OnComplete(eventHolder.EndEvent).SetDelay(delay.DelayTime)
                        : GetTween(ActionTime(actionTime)).SetEase(ease.AnimationCurveEase)
                            .OnComplete(eventHolder.EndEvent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return CurrentTween;
        }

        /// <summary>
        /// Returns the customized action tween.
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        public abstract Tween GetTween(float actionTime);

        /// <summary>
        /// Returns the time amount for action to be completed.
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        public float ActionTime(float actionTime)
        {
            return time.OverrideTime ? time.ActionTime : actionTime;
        }

        public void Stop(bool complete = false)
        {
            CurrentTween?.Kill(complete);
        }

        public abstract void SetInitialState();

        public abstract void ResetPreviousState();

        public abstract void SaveObjectValues();

        /// <summary>
        /// Returns the type of the action.
        /// </summary>
        /// <returns></returns>
        public abstract ActionType Type();

        /// <summary>
        /// Returns the component type of the action.
        /// </summary>
        /// <returns></returns>
        public abstract MovableComponentExtension.ComponentType GetComponentType();

        /// <summary>
        /// Copies specified action values to the current one.
        /// </summary>
        /// <param name="actionToCopyFrom"></param>
        /// <returns></returns>
        public virtual MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (actionToCopyFrom == null)
                return this;

            time = actionToCopyFrom.time;
            delay = actionToCopyFrom.delay;
            ease = actionToCopyFrom.ease;
            eventHolder = actionToCopyFrom.eventHolder;

            return this;
        }

        /// <summary>
        /// Returns the warning if the wrong component was assigned.
        /// </summary>
        /// <param name="assignedComponent"></param>
        /// <returns></returns>
        protected string Warning<T>(Component assignedComponent)
        {
            return assignedComponent == null
                ? $"No component of type {assignedComponent.GetType()} was found on the object"
                : $"Wrong component was assigned: {assignedComponent.GetType()}. Expected: {typeof(T)}";
        }

        public object Clone()
        {
            var clone = (MovableAction) MemberwiseClone();

            clone.time = (MovableTime) clone.time.Clone();
            clone.delay = (MovableDelay) clone.delay.Clone();
            clone.ease = (MovableEase) clone.ease.Clone();
            clone.eventHolder = (MovableEventsHolder) clone.eventHolder.Clone();

            return clone;
        }

        public enum ActionType
        {
            None = 0,
            Position = 1,
            Rotation = 2,
            Scale = 3,
            Size = 4,
            Color = 5,
            Transparency = 6,

            //Scriptable actions are sent back for
            //easy possibility to add new actions without needing to change indexing.
            ScriptableAction = 100
        }
    }
}