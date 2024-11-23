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

        public override object GetValue(Entity source, Context context)
        {
            if (source.TryGetCachedComponent<StatisticRepository>(out StatisticRepository statisticRepository) && statisticRepository.TryGet(name, out Statistic statistic))
            {
                StatisticDefinition definition = overrideDefinitionDescriptor != null ? overrideDefinitionDescriptor : statistic.Definition;
                string formattedValue = statistic.GetFormattedValue(definition != null ? definition.Format : string.Empty, context);

                if (statistic.TryGetDescription(out string description, context))
                    return AddDefinitionFormat($"({description}) ({formattedValue})", definition);
                else
                    return AddDefinitionFormat($"({formattedValue})", definition);
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
