using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StandardStateStatisticDefinition", menuName = "Definition/Statistic/StandardStateStatisticDefinition")]
    public class StandardStateStatisticDefinition : StatisticDefinition<bool>
    {
        public override bool Modify(bool value, StatisticRepository repository)
        {
            return value;
        }
    }
}
