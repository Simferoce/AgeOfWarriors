using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticPercentage : Statistic<float>
    {
        [SerializeField] private StatisticReference<float> reference;
        [SerializeField, Range(0, 5)] private float percentage;

        public override string GetDescription()
        {
            return $"<color=#{reference?.Definition?.ColorHex}>({percentage * 100} % of {reference?.Definition?.Title ?? "Undefined"})</color>";
        }

        public override float GetValue(object caller)
        {
            return reference.TryGetValue(caller, out float value) == true ? value * percentage : 0f;
        }
    }
}
