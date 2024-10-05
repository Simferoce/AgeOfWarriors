using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierParameterStatistic : ModifierParameter, IStatisticContext
    {
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public IEnumerable<Statistic> GetStatistic()
        {
            yield return statistic;
        }
    }
}
