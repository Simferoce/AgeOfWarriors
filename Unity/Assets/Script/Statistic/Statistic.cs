using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticDefinition definition;

        public string Name { get => name; set => name = value; }
        public StatisticDefinition Definition { get => definition; set => definition = value; }

        public abstract string GetDescription(object caller);
        public abstract string GetValueText(object caller);
    }

    [Serializable]
    public abstract class Statistic<T> : Statistic
    {
        public abstract T GetValue(object caller);

        public override string GetValueText(object caller)
        {
            return GetValue(caller).ToString();
        }
    }
}
