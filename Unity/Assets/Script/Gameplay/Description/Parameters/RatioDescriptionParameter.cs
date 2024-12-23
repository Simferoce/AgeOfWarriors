using Game.Statistics;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class RatioDescriptionParameter : DescriptionParameter
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticDefinition statisticDefinitionDescriptor;
        [SerializeField] private StatisticDefinition ratioDefinitionDescriptor;
        [SerializeReference, SubclassSelector] private IFloatAdjustment adjustment;

        public override object GetValue(Entity source)
        {
            if (source.StatisticRepository.TryGet(name, out Statistic statistic))
            {
                float value = statistic.Get<float>();
                if (adjustment != null)
                    value = adjustment.Adjust(value);

                value = (float)Math.Round(value, 2);
                string formattedValue = value.ToString("0.0%");

                return AddDefinitionFormat($"({formattedValue}{ratioDefinitionDescriptor.TextIcon})", statisticDefinitionDescriptor);
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
