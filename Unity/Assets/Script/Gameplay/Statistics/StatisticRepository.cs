using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    public class StatisticRepository : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;

        public IReadOnlyList<Statistic> Statistics => statistics;

        public void Add(Statistic statistic)
        {
            statistics.Add(statistic);
        }

        public void Remove(Statistic statistic)
        {
            statistics.Remove(statistic);
        }

        public bool TryGet(StatisticDefinition definition, out Statistic statistic)
        {
            statistic = statistics.FirstOrDefault(x => x.Definition == definition);
            return statistic != null;
        }
    }
}
