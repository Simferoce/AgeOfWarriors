using System;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Value
    {
        public virtual bool ExpressiveDescription => true;

        protected Entity owner;

        public virtual void Initialize(Entity owner)
        {
            this.owner = owner;
        }

        public abstract T GetValue<T>();
        public abstract Value Snapshot();

        public abstract string GetDescription(Context context);
    }

    [Serializable]
    public abstract class Value<ReferenceType> : Value
    {
        public abstract ReferenceType GetValue();

        public override Value Snapshot()
        {
            return new SerializeValue<ReferenceType>() { Value = GetValue() };
        }

        public override T GetValue<T>()
        {
            return StatisticConverter.ConvertGeneric<T, ReferenceType>(GetValue());
        }
    }
}
