using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierBehaviourStatistic : ModifierBehaviour, IStatisticContext
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;

        public override void Initialize(Modifier modifier)
        {
            base.Initialize(modifier);

            foreach (Statistic statistic in statistics)
                statistic.Initialize(modifier);
        }

        public IEnumerable<Statistic> GetStatistic()
        {
            foreach (Statistic statistic in statistics)
                yield return statistic;
        }
    }
}