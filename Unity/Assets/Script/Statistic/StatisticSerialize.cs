using System;
using UnityEngine;

namespace Game
{
    [Serializable] public class StatisticSerializeFloat : StatisticSerialize<float> { }

    [Serializable]
    public class StatisticSerialize<T> : Statistic<T>
    {
        [SerializeField] private T value;

        public override string GetDescription(object caller)
        {
            return value.ToString();
        }

        public override T GetValue(object caller)
        {
            return value;
        }
    }
}
