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

        public override Statistic Snapshot(Context context)
        {
            return new StandardStateStatistic() { definition = this.definition, Value = new SerializeValue<bool>() { Value = GetModifiedValue(context) } };
        }

        public override bool GetModifiedValue(Context context)
        {
            return definition != null ? definition.Modify(value.GetValue<bool>(), entity.GetCachedComponent<StatisticRepository>(), context) : GetBaseValue(context);
        }
        public override bool GetBaseValue(Context context)
        {
            return value.GetValue<bool>();
        }

        public override bool TryGetDescription(out string description, Context context)
        {
            return value.TryGetDescription(out description);
        }

        public override string GetFormattedValue(string format, Context context)
        {
            return value.GetValue<bool>().ToString();
        }
    }
}
