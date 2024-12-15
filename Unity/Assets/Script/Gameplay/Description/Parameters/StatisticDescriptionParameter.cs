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
        [SerializeField] private bool asPercentage;
        [SerializeReference, SubclassSelector] private IFloatAdjustment adjustment;

        public override object GetValue(Entity source)
        {
            if (source.StatisticRepository.TryGet(name, out Statistic statistic))
            {
                StatisticDefinition definition = overrideDefinitionDescriptor != null ? overrideDefinitionDescriptor : statistic.Definition;
                float value = statistic.Get<float>();
                if (adjustment != null)
                    value = adjustment.Adjust(value);

                string formattedValue = asPercentage ? value.ToString("0.0%") : value.ToString();

                if (statistic.TryGetDescription(out string description))
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
