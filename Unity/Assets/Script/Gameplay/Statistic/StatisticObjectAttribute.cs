using System;

public class StatisticObjectAttribute : Attribute
{
    public string Name { get; set; }

    public StatisticObjectAttribute(string name)
    {
        Name = name;
    }
}