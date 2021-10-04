using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject.Actions
{
    public class MovableActionMeshRendererTransparency : MovableActionMeshRenderer, IMovableActionCurrentState,
        IMovableTransparency
    {
        [OdinSerialize]
        [PropertyRange(0f, 1f)]
        public float Alpha { get; set; }

        public float PreviousAlpha { get; set; }

        public MovableActionMeshRendererTransparency()
        {
        }

        public MovableActionMeshRendererTransparency(MeshRenderer meshRenderer) : base(meshRenderer)
        {
        }

        protected override void ResetDefaultValues()
        {
            Alpha = 0f;
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
            return ActionType.Transparency;
        }

        public override MovableAction Copy(MovableAction actionToCopyFrom)
        {
            if (actionToCopyFrom is IMovableTransparency actionToCopyTransparency)
                Alpha = actionToCopyTransparency.Alpha;

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