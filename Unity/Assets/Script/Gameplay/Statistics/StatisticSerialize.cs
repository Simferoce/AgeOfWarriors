using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Statistics
{
    [Serializable]
    [MovedFrom(true, sourceClassName: "StatisticSerialize")]
    public class StatisticSerializeFloat : StatisticSerialize<float>
    {
        public override string GetDescription(object context)
        {
            if (definition.IsPercentage)
                return $"<color=#{definition.ColorHex}>({value:0.0%}{definition.TextIcon})</color>";
            else
                return $"<color=#{definition.ColorHex}>({value}{definition.TextIcon})</color>";
        }
    }

    [Serializable]
    public class StatisticSerializeBool : StatisticSerialize<bool>
    {
        public override string GetDescription(object context)
        {
            return $"<color=#{definition.ColorHex}>({value}{definition.TextIcon})</color>";
        }
    }

    [Serializable]
    public abstract class StatisticSerialize<T> : Statistic
    {
        [SerializeField] protected T value;

        public override U GetValue<U>(object context)
        {
            return StatisticConverter.ConvertGeneric<U, T>(value);
        }
    }
}
