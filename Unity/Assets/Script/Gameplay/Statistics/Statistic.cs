using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected string name;
        [SerializeField] protected StatisticDefinition definition;

        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public string Name { get => name; set => name = value; }

        public abstract T GetValue<T>(object context);
        public abstract string GetDescription(object context);
    }
}