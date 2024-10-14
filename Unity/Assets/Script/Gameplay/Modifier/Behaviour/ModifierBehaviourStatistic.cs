using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierBehaviourStatistic : ModifierBehaviour
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;

        public override void Initialize(Modifier modifier)
        {
            base.Initialize(modifier);

            StatisticIndex statisticIndex = modifier.AddOrGetCachedComponent<StatisticIndex>();
            foreach (Statistic statistic in statistics)
                statisticIndex.Add(statistic);
        }
    }
}