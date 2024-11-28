namespace Game
{
    public abstract class Statistic
    {
        public string Name { get => name; set => name = value; }
        public StatisticDefinition Definition { get => definition; set => definition = value; }

        private StatisticDefinition definition;
        private string name;

        public Statistic(StatisticDefinition definition)
        {
            this.definition = definition;
            this.name = definition.HumanReadableId;
        }

        public Statistic(string name, StatisticDefinition definition)
        {
            this.definition = definition;
            this.name = name;
        }

        public static implicit operator float(Statistic statistic) => statistic.GetValue<float>();

        public abstract T GetValue<T>();
    }

    public class Statistic<T> : Statistic
    {
        private T value;

        public Statistic(StatisticDefinition definition, T value = default) : base(definition)
        {
            this.value = value;
        }

        public Statistic(string name, StatisticDefinition definition, T value = default) : base(name, definition)
        {
            this.value = value;
        }

        public T GetValue()
        {
            return value;
        }

        public override T1 GetValue<T1>()
        {
            return StatisticUtility.ConvertGeneric<T1, T>(GetValue());
        }

        public void SetValue(T value)
        {
            this.value = value;
        }
    }
}
