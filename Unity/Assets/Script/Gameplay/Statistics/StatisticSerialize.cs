using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticSerialize : GameStatisticSerialize<float> { }

    [Serializable]
    public class GameStatisticSerialize<T> : Statistic
    {
        [SerializeField] private T value;

        public override U GetValue<U>(object context)
        {
            return StatisticConverter.ConvertGeneric<U, T>(value);
        }
    }
}
