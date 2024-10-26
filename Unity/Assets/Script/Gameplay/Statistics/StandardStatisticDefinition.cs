using System.Collections.Generic;
using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StandardStatisticDefinition", menuName = "Definition/Statistic/StandardStatisticDefinition")]
    public class StandardStatisticDefinition : StatisticDefinition
    {
        [SerializeField] private List<StatisticDefinition> flat;
        [SerializeField] private List<StatisticDefinition> percentage;
        [SerializeField] private List<StatisticDefinition> multiplier;
        [SerializeField] private List<StatisticDefinition> maximum;
        [SerializeField] private List<StatisticDefinition> minimum;

        public List<StatisticDefinition> Flat { get => flat; set => flat = value; }
        public List<StatisticDefinition> Percentage { get => percentage; set => percentage = value; }
        public List<StatisticDefinition> Multiplier { get => multiplier; set => multiplier = value; }
        public List<StatisticDefinition> Maximum { get => maximum; set => maximum = value; }
        public List<StatisticDefinition> Minimum { get => minimum; set => minimum = value; }

        public override Statistic BuildStatistic()
        {
            return new StandardStatistic() { Definition = this, Name = this.HumanReadableId };
        }
    }
}
