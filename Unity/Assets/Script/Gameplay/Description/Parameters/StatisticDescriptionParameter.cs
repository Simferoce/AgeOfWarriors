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

        public override object GetValue(Entity source, bool showValue)
        {
            if (source.StatisticRepository.TryGet(name, out Statistic statistic))
            {
                StatisticDefinition definition = overrideDefinitionDescriptor != null ? overrideDefinitionDescriptor : statistic.Definition;
                bool hasDescription = statistic.TryGetDescription(out string description);
                if (hasDescription && showValue)
                    return AddDefinitionFormat($"({description}) ({GetFormattedValue(statistic)})", definition);
                else if (hasDescription && !showValue)
                    return AddDefinitionFormat($"({description})", definition);
                else
                    return AddDefinitionFormat($"({GetFormattedValue(statistic)})", definition);
            }

            return $"{{{name}}}";
        }

        private string GetFormattedValue(Statistic statistic)
        {
            float value = statistic.Get<float>();
            if (adjustment != null)
                value = adjustment.Adjust(value);

            value = (float)Math.Round(value, 2);
            string formattedValue = asPercentage ? value.ToString("0.0%") : value.ToString();
            return formattedValue;
        }

        private string AddDefinitionFormat(string value, StatisticDefinition definition)
        {
            if (definition == null)
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(Color.white)}>{value}</color>";

            return $"<color=#{definition.ColorHex}>{value}</color>";
        }
    }
}
