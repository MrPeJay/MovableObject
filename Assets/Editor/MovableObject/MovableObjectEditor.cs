using DG.DOTweenEditor;
using DG.Tweening;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace MovableObject.Preview.Editor
{
    [CustomEditor(typeof(MovableObjectTwoStage))]
    public class MovableObjectEditor : OdinEditor
    {
        /// <summary>
        /// Prepares tween for preview and instantly plays it.
        /// </summary>
        /// <param name="tween"></param>
        public static void PreviewTween(Tween tween)
        {
            DOTweenEditorPreview.PrepareTweenForPreview(tween);
            DOTweenEditorPreview.Start();
        }

        private MovableObjectBase _objectTarget;

        protected override void OnEnable()
        {
            _objectTarget = target as MovableObjectBase;
        }

        protected override void OnDisable()
        {
            if (_objectTarget == null) return;

            //Stop preview mode when deselected.
            if (!_objectTarget.IsPreviewMode()) return;

            _objectTarget.StopPreviewMode();
            DOTweenEditorPreview.Stop();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (_objectTarget.IsPreviewMode())
            {
                GUI.enabled = false;
                EditorGUILayout.IntField("Current stage index", _objectTarget.CurrentStageIndex());
                GUI.enabled = true;

                if (GUILayout.Button("Next Stage Preview"))
                    PreviewTween(_objectTarget.PlayAction());

                if (!GUILayout.Button("Stop Preview Mode")) return;

                DOTweenEditorPreview.Stop(true);
                _objectTarget.StopPreviewMode();
            }
            else
            {
                if (GUILayout.Button("Start Preview Mode"))
                    _objectTarget.StartPreviewMode();
            }
        }
    }
}