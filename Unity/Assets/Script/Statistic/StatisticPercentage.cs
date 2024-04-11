using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticPercentage : Statistic<float>
    {
        [SerializeField] private StatisticReference<float> reference;
        [SerializeField, Range(0, 5)] private float percentage;

        public override string GetDescriptionFormatted(object caller)
        {
            string info = reference.Definition != null ? $"(<color=#{Definition?.ColorHex}>{percentage * 100}%{reference.Definition.GetTextIcon}</color>)" : "";
            return $"<color=#{Definition?.ColorHex}>{GetValue(caller)}{info}</color>";
        }

        public override string GetDescription()
        {
            return $"({percentage * 100} % of {reference?.Definition?.Title ?? "Undefined"})";
        }

        public override float GetValue(object caller)
        {
            return reference.TryGetValue(caller, out float value) == true ? value * percentage : 0f;
        }
    }
}
