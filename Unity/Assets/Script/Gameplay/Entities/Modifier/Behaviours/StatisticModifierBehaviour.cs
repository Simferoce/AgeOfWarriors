using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private StatisticDefinition definition;
        [SerializeReference, SubclassSelector] private Value value;

        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public Value Value { get => value; set => this.value = value; }

        private Statistic statistic;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            value.Initialize(modifier);
            statistic = new Statistic() { Definition = definition, Value = Value };

            StatisticRepository statisticRepository = modifier.Target.Entity.GetCachedComponent<StatisticRepository>();
            statisticRepository.Add(statistic);
        }

        public override void Dispose()
        {
            base.Dispose();

            StatisticRepository statisticRepository = modifier.Target.Entity.GetCachedComponent<StatisticRepository>();
            statisticRepository.Remove(statistic);
        }
    }
}
