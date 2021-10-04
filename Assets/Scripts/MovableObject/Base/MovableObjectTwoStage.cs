using MovableObject.Staging;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace MovableObject
{
    public class MovableObjectTwoStage : MovableObjectMultiStage
    {
        [OdinSerialize]
        [ListDrawerSettings(IsReadOnly = true)]
        protected override MovableStage[] Stages
        {
            get
            {
                if (_stages == null)
                    _stages = new MovableStage[] {new MovableStage(this), new MovableStage(this)};

                return _stages;
            }
            set => _stages = value;
        }

        [SerializeField] [HideInInspector] private MovableStage[] _stages;
    }
}