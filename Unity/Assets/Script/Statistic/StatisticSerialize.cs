using System;
using UnityEngine;

namespace Game
{
    [Serializable] public class StatisticSerializeFloat : StatisticSerialize<float> { }

    [Serializable]
    public class StatisticSerialize<T> : Statistic<T>
    {
        [SerializeField] private T value;

        public override string GetDescriptionFormatted(object caller)
        {
            return $"<color=#{Definition?.ColorHex}>{GetValue(caller)}</color>";
        }

        public override string GetDescription()
        {
            return "";
        }

        public override T GetValue(object caller)
        {
            return value;
        }
    }
}
