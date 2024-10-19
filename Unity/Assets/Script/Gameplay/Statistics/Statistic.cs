using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected StatisticDefinition definition;

        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public abstract T GetValue<T>(object context);
    }
}