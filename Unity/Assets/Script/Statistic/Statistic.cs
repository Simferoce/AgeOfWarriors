using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField, HideInInspector] protected string description;
        [SerializeField] private StatisticDefinition definition;

        public StatisticDefinition Definition { get => definition; set => definition = value; }

        public void SetDescription()
        {
            description = $"{Definition?.Title ?? "Undefined"} - {GetDescription()}";
        }

        public abstract string GetDescriptionFormatted(object caller);
        public abstract string GetDescription();
    }

    [Serializable]
    public abstract class Statistic<T> : Statistic
    {
        public abstract T GetValue(object caller);
    }
}
