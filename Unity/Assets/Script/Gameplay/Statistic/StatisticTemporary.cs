using Game;

public class StatisticTemporary<Type> : Statistic
{
    public override string Name { get; set; }
    public override StatisticDefinition Definition { get; set; }

    private Type value;

    public StatisticTemporary(IStatisticContext context, string name, Type value, StatisticDefinition definition = null)
    {
        this.context = context;
        Name = name;
        Definition = definition;
        this.value = value;
    }

    public override bool TryGetValue<T>(out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, Type>(this.value);
        return true;
    }
}