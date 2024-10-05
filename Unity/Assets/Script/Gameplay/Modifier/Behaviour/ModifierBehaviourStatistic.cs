using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierBehaviourStatistic : ModifierBehaviour, IStatisticContext
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;

        public IEnumerable<Statistic> GetStatistic()
        {
            foreach (Statistic statistc in statistics)
                yield return statistc;
        }
    }
}