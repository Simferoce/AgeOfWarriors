using Game.Statistics;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDescriptionParameter : DescriptionParameter
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticDefinition overrideDefinitionDescriptor;

        public override object GetValue(Entity source)
        {
            if (source.TryGetCachedComponent<StatisticRepository>(out StatisticRepository statisticRepository) && statisticRepository.TryGet(name, out Statistic statistic))
            {
                StatisticDefinition definition = overrideDefinitionDescriptor != null ? overrideDefinitionDescriptor : statistic.Definition;

                if (statistic.TryGetDescription(out string description))
                    return AddDefinitionFormat($"({description}) ({statistic.GetFormattedValue()})", definition);
                else
                    return AddDefinitionFormat($"({statistic.GetFormattedValue()})", definition);
            }

            return $"{{{name}}}";
        }

        private string AddDefinitionFormat(string value, StatisticDefinition definition)
        {
            if (definition == null)
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(Color.white)}>{value}</color>";

            return $"<color=#{definition.ColorHex}>{value}</color>";
        }
    }
}
