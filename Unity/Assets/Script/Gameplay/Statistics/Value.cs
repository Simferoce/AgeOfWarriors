using System;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Value
    {
        protected StatisticRepository owner;

        public virtual void Initialize(StatisticRepository owner)
        {
            this.owner = owner;
        }

        public abstract T GetValue<T>();


    }

    [Serializable]
    public abstract class Value<ReferenceType> : Value
    {
        public abstract ReferenceType GetValue();

        public override T GetValue<T>()
        {
            return StatisticUtility.ConvertGeneric<T, ReferenceType>(GetValue());
        }
    }
}
