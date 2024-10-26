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

        public override T GetValue(Context context)
        {
            return value;
        }
    }
}
