using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticReferenceValue<T> : Value<T>
    {
        [SerializeField] private StatisticReference<T> reference;

        public override void Initialize(StatisticRepository owner)
        {
            base.Initialize(owner);
            reference.Initialize(owner.Owner as Entity);
        }

        public override T GetValue()
        {
            return reference.Get().Get<T>();
        }
    }

    [Serializable]
    public class StatisticReferenceValueFloat : StatisticReferenceValue<float>
    {

    }
}
