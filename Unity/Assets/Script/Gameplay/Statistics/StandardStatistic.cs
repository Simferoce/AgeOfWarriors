using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StandardStatistic : Statistic<float>
    {
        [SerializeReference, SubclassSelector] private Value value;
        public Value Value { get => value; set => this.value = value; }

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            value.Initialize(entity);
        }

        public override Statistic Snapshot()
        {
            return new StandardStatistic() { definition = this.definition, Value = new SerializeValue<float>() { Value = GetModifiedValue() } };
        }

        public override float GetModifiedValue()
        {
            return definition != null ? definition.Modify(value.GetValue<float>(), entity.GetCachedComponent<StatisticRepository>()) : GetBaseValue();
        }

        public override float GetBaseValue()
        {
            return value.GetValue<float>();
        }

        public override bool TryGetDescription(out string description)
        {
            return value.TryGetDescription(out description);
        }

        public override string GetFormattedValue()
        {
            return value.GetValue<float>().ToString();
        }
    }
}
