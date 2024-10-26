using System.Collections.Generic;
using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "ModifiableStatisticDefinition", menuName = "Definition/Statistic/ModifiableStatisticDefinition")]
    public class ModifiableStatisticDefinition : StatisticDefinition
    {
        [SerializeField] private List<StatisticDefinition> maximum;
        [SerializeField] private List<StatisticDefinition> minimum;

        public List<StatisticDefinition> Maximum { get => maximum; set => maximum = value; }
        public List<StatisticDefinition> Minimum { get => minimum; set => minimum = value; }

        public override Statistic BuildStatistic()
        {
            return new StatisticModifiable() { Definition = this, Name = this.HumanReadableId };
        }
    }
}
