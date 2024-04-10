using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Statistic
    {
        [SerializeField, HideInInspector] protected string description;
        [SerializeField] private StatisticDefinition definition;

        public StatisticDefinition Definition { get => definition; set => definition = value; }

        public void SetDescription()
        {
            description = $"{Definition?.Title ?? "Undefined"} - {GetDescription()}";
        }

        public virtual string GetDescription() { return ""; }
        public virtual string GetValueText(object caller) { return ""; }
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
