using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class StatisticClassAttribute : Attribute
{
    public string Name { get; set; }

    public StatisticClassAttribute()
    {
    }

    public StatisticClassAttribute(string name)
    {
        Name = name;
    }
}
