using System;
using UnityEngine;

namespace Game
{
    [Serializable] public class StatisticSerializeFloat : StatisticSerialize<float> { }

    [Serializable]
    public class StatisticSerialize<T> : Statistic<T>
    {
        [SerializeField] private T value;
        [SerializeField] private bool percentage;

        public override string GetDescriptionFormatted(object caller)
        {
            if (percentage == true)
                return $"{(float)(object)GetValue(caller) * 100}%";
            else
                return GetValue(caller).ToString();
        }

        public override string GetDescription()
        {
            if (percentage == true)
                return $"{(float)(object)value * 100}%";
            else
                return value.ToString();
        }

        public override T GetValue(object caller)
        {
            return value;
        }
    }
}
