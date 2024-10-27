using System;

namespace Game.Statistics
{
    [Serializable]
    public abstract class StatisticValue
    {
        public virtual bool ExpressiveDescription => true;

        protected Entity owner;

        public virtual void Initialize(Entity owner)
        {
            this.owner = owner;
        }

        public abstract T GetValue<T>(Context context);

        public abstract string GetDescription(Context context);
    }

    [Serializable]
    public abstract class StatisticValue<ReferenceType> : StatisticValue
    {
        public abstract ReferenceType GetValue(Context context);

        public override T GetValue<T>(Context context)
        {
            return StatisticConverter.ConvertGeneric<T, ReferenceType>(GetValue(context));
        }
    }
}
