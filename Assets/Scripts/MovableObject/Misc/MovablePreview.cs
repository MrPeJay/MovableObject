using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MovableObject.Preview
{
    [Serializable]
    [InlineProperty]
    [HideLabel]
    [HideReferenceObjectPicker]
    public class MovablePreview
    {
        private bool _isPreviewing;

        [SerializeField] [HideInInspector] private MovableObjectBase _movableObject;

        public MovablePreview(MovableObjectBase movableObject)
        {
            _movableObject = movableObject;
        }

        /// <summary>
        /// Returns whether preview is taking place.
        /// </summary>
        /// <returns></returns>
        public bool IsPreviewing()
        {
            return _isPreviewing;
        }

        /// <summary>
        /// Starts movable object animation preview mode.
        /// </summary>
        public void StartPreviewMode()
        {
            _isPreviewing = true;

            //Save current values.
            _movableObject.SaveObjectValues();
            _movableObject.SetInitialState();
        }

        /// <summary>
        /// Stops movable object animation preview mode.
        /// </summary>
        public void StopPreviewMode()
        {
            _isPreviewing = false;

            _movableObject.Stop();
            _movableObject.ResetPreviousState();
        }
    }
}