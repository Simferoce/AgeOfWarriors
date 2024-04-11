using System;
using UnityEngine;

namespace Game
{
    [Serializable] public class StatisticSerializeFloat : StatisticSerialize<float> { }

    [Serializable]
    public class StatisticSerialize<T> : Statistic<T>
    {
        [SerializeField] private T value;
        [SerializeField] private bool showIcon = false;

        public override string GetDescriptionFormatted(object caller)
        {
            if (showIcon)
            {
                string icon = Definition?.Icon != null ? $"<sprite name=\"{Definition.Icon.name.Trim()}\" color=#{Definition.ColorHex}>" : "";
                return $"<color=#{Definition?.ColorHex}>{GetValue(caller)}</color>{icon}";
            }
            else
            {
                return GetValue(caller).ToString();
            }
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
