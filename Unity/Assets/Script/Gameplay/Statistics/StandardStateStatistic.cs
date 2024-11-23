using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StandardStateStatistic : Statistic<bool>
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
            return new StandardStateStatistic() { definition = this.definition, Value = new SerializeValue<bool>() { Value = GetModifiedValue() } };
        }
        public override bool GetModifiedValue()
        {
            return definition != null ? definition.Modify(value.GetValue<bool>(), entity.GetCachedComponent<StatisticRepository>()) : GetBaseValue();
        }
        public override bool GetBaseValue()
        {
            return value.GetValue<bool>();
        }

        public override bool TryGetDescription(out string description)
        {
            return value.TryGetDescription(out description);
        }

        public override string GetFormattedValue(string format)
        {
            return value.GetValue<bool>().ToString();
        }
    }
}
