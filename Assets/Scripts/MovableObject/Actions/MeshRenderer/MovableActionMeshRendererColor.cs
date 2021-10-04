using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionMeshRendererColor : MovableActionMeshRenderer, IMovableActionCurrentState, IMovableColor
    {
        [OdinSerialize] public Color Color { get; set; }
        public Color PreviousColor { get; set; }

        public MovableActionMeshRendererColor() { }

        public MovableActionMeshRendererColor(MeshRenderer meshRenderer) : base(meshRenderer)
        {
        }

        protected override void ResetDefaultValues()
        {
            Color = new Color();
        }

        public override Tween GetTween(float actionTime)
        {
            throw new System.NotImplementedException();
        }

        public override void SetInitialState()
        {
            throw new System.NotImplementedException();
        }

        public override void ResetPreviousState()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveObjectValues()
        {
            throw new System.NotImplementedException();
        }

        public override ActionType Type()
        {
            return ActionType.Color;
        }

        public override MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (actionToCopyFrom is IMovableColor colorAction)
                Color = colorAction.Color;

            return base.Copy(actionToCopyFrom);
        }

        [ShowIf("@ContainsComponents()")]
        [Button]
        [PropertyOrder(1)]
        public void SetCurrentState()
        {
            throw new System.NotImplementedException();
        }
    }
}