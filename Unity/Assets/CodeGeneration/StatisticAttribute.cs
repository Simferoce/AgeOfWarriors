using System;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class StatisticAttribute : Attribute
{
    public string Name { get; set; }
    public bool AppendStatisticClassName { get; set; }

    public StatisticAttribute()
    {

    }

    public StatisticAttribute(string name, bool appendStatisticClassName = false)
    {
        Name = name;
        AppendStatisticClassName = appendStatisticClassName;
    }
}