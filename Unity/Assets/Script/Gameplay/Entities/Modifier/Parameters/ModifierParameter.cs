using Game.Statistics;

namespace Game.Modifier
{
    public abstract class ModifierParameter
    {
        public virtual string Name { get; set; }

        protected ModifierParameter(string name)
        {
            Name = name;
        }

        public abstract T GetValue<T>();

        public abstract ModifierParameter Clone();
    }

    public class ModifierParameter<ReferenceType> : ModifierParameter
    {
        public ReferenceType value;

        public ModifierParameter(string name, ReferenceType value) : base(name)
        {
            this.value = value;
        }

        public override ModifierParameter Clone()
        {
            return new ModifierParameter<ReferenceType>(Name, value);
        }

        public ReferenceType GetValue()
        {
            return value;
        }

        public override T GetValue<T>()
        {
            return StatisticUtility.ConvertGeneric<T, ReferenceType>(GetValue());
        }
    }
}