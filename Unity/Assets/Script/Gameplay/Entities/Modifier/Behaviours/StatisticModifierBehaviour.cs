using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierBehaviour<T> : ModifierBehaviour
    {
        [SerializeField] private StatisticDefinition<T> definition;
        [SerializeField] private StatisticReference<T> reference;

        private Statistic statistic;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            statistic = new StandardStatistic() { Definition = definition, Entity = modifier, Value = new EntityReferenceValue<T>() { Reference = reference } };
            statistic.Initialize(modifier);

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

    [Serializable]
    public class StatisticModifierBehaviourFloat : StatisticModifierBehaviour<float> { }

    [Serializable]
    public class StatisticModifierBehaviourBool : StatisticModifierBehaviour<bool> { }
}
