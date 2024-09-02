using System;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class StatisticAttribute : Attribute
{
    public string Name { get; set; }

    public StatisticAttribute(string name)
    {
        Name = name;
    }
}