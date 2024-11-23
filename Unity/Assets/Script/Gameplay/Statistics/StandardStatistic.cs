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

        public override Statistic Snapshot(Context context)
        {
            return new StandardStatistic() { definition = this.definition, Value = new SerializeValue<float>() { Value = GetModifiedValue(context) } };
        }

        public override float GetModifiedValue(Context context)
        {
            return definition != null ? definition.Modify(value.GetValue<float>(), entity.GetCachedComponent<StatisticRepository>(), context) : GetBaseValue(context);
        }

        public override float GetBaseValue(Context context)
        {
            return value.GetValue<float>();
        }

        public override bool TryGetDescription(out string description, Context context)
        {
            return value.TryGetDescription(out description);
        }

        public override string GetFormattedValue(string format, Context context)
        {
            return value.GetValue<float>().ToString(format);
        }
    }
}
