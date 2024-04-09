using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] private string title;

        protected object owner;

        public string Title { get => title; set => title = value; }

        public void Initialize(object owner)
        {
            this.owner = owner;
        }
    }

    [Serializable]
    public abstract class Statistic<T> : Statistic
    {
        public abstract StatisticDefinition GetDefinition(StatisticContext context);
        public abstract T GetValue(StatisticContext context);

        public T GetValue()
        {
            return GetValue(new StatisticContext());
        }
    }

    public interface IStatisticFloat
    {
        public abstract float GetValue();
        public abstract float GetValue(StatisticContext context);
    }
}
