using System;
using DG.Tweening;
using MovableObject.Misc;
using MovableObject.Preview;
using Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject
{
    public abstract class MovableObjectBase : SerializedMonoBehaviour, IMovableTween
    {
        private const string SettingsGroup = "General Settings";

        public Tween CurrentTween { get; set; }

        [FoldoutGroup(SettingsGroup)] [VerticalGroup(SettingsGroup + "/VerticalGroup")] [SerializeField]
        private MovableTime time;

        [VerticalGroup(SettingsGroup + "/VerticalGroup")] [SerializeField]
        protected bool UseUnscaledTime;

        [HorizontalGroup(SettingsGroup + "/VerticalGroup/HorizontalGroup", .5f)]
        [Tooltip("Automatically invokes PlayAction() method which starts the animation on start.")]
        [SerializeField]
        protected bool autoStart;

        [HorizontalGroup(SettingsGroup + "/VerticalGroup/HorizontalGroup", .5f)]
        [Tooltip("Automatically loops between stages without user intervention.")]
        [SerializeField]
        protected bool loop;

        private MovablePreview Preview
        {
            get
            {
                if (_preview == null)
                    _preview = new MovablePreview(this);

                return _preview;
            }
        }

        private MovablePreview _preview;

        protected virtual void Start()
        {
            if (autoStart)
                PlayAction();
        }

        /// <summary>
        /// Returns whether movable object is in preview mode.
        /// </summary>
        /// <returns></returns>
        public bool IsPreviewMode()
        {
            return Preview.IsPreviewing();
        }

        /// <summary>
        /// Plays current stage animations;
        /// </summary>
        /// <returns></returns>
        public abstract Tween PlayAction();

        public abstract void SetInitialState();

        public abstract void ResetPreviousState();

        public abstract void SaveObjectValues();

        /// <summary>
        /// Returns current stage index.
        /// </summary>
        /// <returns></returns>
        public abstract int CurrentStageIndex();

        /// <summary>
        /// Starts movable object animation preview mode.
        /// </summary>
        public void StartPreviewMode()
        {
            Preview.StartPreviewMode();
        }

        /// <summary>
        /// Stops movable object animation preview mode.
        /// </summary>
        public void StopPreviewMode()
        {
            Preview.StopPreviewMode();
        }

        /// <summary>
        /// Returns the action time to be completed.
        /// </summary>
        /// <returns></returns>
        public float ActionTime()
        {
            var settings = GameSettings.Instance.MovableObjectSettings;

            return time.OverrideTime ? time.ActionTime : settings.DefaultActionTime;
        }

        [Serializable]
        public struct Settings
        {
            public float DefaultActionTime;
        }

        public void Stop(bool complete = false)
        {
            CurrentTween?.Kill(complete);
        }
    }
}