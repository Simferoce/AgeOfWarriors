using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StandardStatisticDefinition", menuName = "Definition/Statistic/StandardStatisticDefinition")]
    public class StandardStatisticDefinition : StatisticDefinition<float>
    {
        public override float Modify(float value, StatisticRepository repository)
        {
            float flat = 0f;
            float percentage = 0f;
            float multiplier = 1f;
            float maximum = float.MaxValue;
            float minimum = float.MinValue;

            foreach (Statistic statistic in repository.Statistics)
            {
                if (statistic.Definition == null)
                    continue;

                ModifyStatisticBehavior modifyStatisticBehavior = statistic.Definition.Behaviors.OfType<ModifyStatisticBehavior>().FirstOrDefault(x => x.Definition == this);
                if (modifyStatisticBehavior == null)
                    continue;

                if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Flat)
                    flat += statistic.GetModifiedValue<float>();
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Pecentage)
                    percentage += statistic.GetModifiedValue<float>();
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Multiplier)
                    multiplier *= statistic.GetModifiedValue<float>();
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Maximum)
                    maximum = Mathf.Min(maximum, statistic.GetModifiedValue<float>());
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Minimum)
                    minimum = Mathf.Max(minimum, statistic.GetModifiedValue<float>());
            }

            return Mathf.Clamp((value + flat) * (1 + percentage) * multiplier, minimum, maximum);
        }
    }
}
