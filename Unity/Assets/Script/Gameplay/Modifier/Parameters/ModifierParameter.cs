namespace Game
{
    public abstract class ModifierParameter
    {
        private string name;

        public string Name { get => name; set => name = value; }

        public ModifierParameter(string name)
        {
            this.name = name;
        }

        public abstract T GetValue<T>();
    }

    public class ModifierParameter<ValueType> : ModifierParameter
    {
        private ValueType value;

        public ModifierParameter(string name, ValueType value) : base(name)
        {
            this.value = value;
        }

        public ValueType GetValue()
        {
            return value;
        }

        public override T GetValue<T>()
        {
            return StatisticUtility.ConvertGeneric<T, ValueType>(value);
        }


        public override string ToString()
        {
            return $"{Name}:{value}";
        }
    }
}
