using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class SerializeStatisticValueFloat : SerializeStatisticValue<float> { }

    [Serializable]
    public class SerializeStatisticValue<T> : StatisticValue<T>
    {
        [SerializeField] private T value;

        public override bool ExpressiveDescription => false;

        public override T GetValue(Context context)
        {
            return value;
        }

        public override string GetDescription(Context context)
        {
            return $"<color=#000000>({value})</color>";
        }
    }
}
