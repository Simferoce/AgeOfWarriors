using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StandardStatistic : Statistic
    {
        [SerializeReference, SubclassSelector] private Value value;
        public Value Value { get => value; set => this.value = value; }

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            value.Initialize(entity);
        }

        public override float GetModifiedValue()
        {
            return definition.Modify(value.GetValue<float>(), entity.GetCachedComponent<StatisticRepository>());
        }

        public override float GetBaseValue()
        {
            return value.GetValue<float>();
        }
    }
}
