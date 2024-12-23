using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ModifierTargetValue : Value<Entity>
    {
        [SerializeReference, SubclassSelector] private ModifierTarget target;

        public override void Initialize(StatisticRepository owner)
        {
            base.Initialize(owner);
            target.Initialize(owner.Owner as ModifierEntity);
        }

        public override Entity GetValue()
        {
            return target.GetTargets()[0] as Entity;
        }
    }
}