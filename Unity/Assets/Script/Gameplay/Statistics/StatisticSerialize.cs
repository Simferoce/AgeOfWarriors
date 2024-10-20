using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Statistics
{
    [Serializable]
    [MovedFrom(true, sourceClassName: "StatisticSerialize")]
    public class StatisticSerializeFloat : StatisticSerialize<float> { }

    [Serializable]
    public class StatisticSerializeBool : StatisticSerialize<bool> { }

    [Serializable]
    public class StatisticSerialize<T> : Statistic
    {
        [SerializeField] private T value;

        public override U GetValue<U>(object context)
        {
            return StatisticConverter.ConvertGeneric<U, T>(value);
        }
    }
}
