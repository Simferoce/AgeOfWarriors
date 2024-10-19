using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierBehaviour : ModifierBehaviour
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            StatisticIndex statisticIndex = modifier.AddOrGetCachedComponent<StatisticIndex>();
            foreach (Statistic statistic in statistics)
                statisticIndex.Add(statistic);
        }
    }
}