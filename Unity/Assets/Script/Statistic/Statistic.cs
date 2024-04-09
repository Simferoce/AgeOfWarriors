using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] private string name;

        public string Name { get => name; set => name = value; }

        public abstract string GetDescription();
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
