using System;

namespace Game
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class StatisticResolveAttribute : Attribute
    {
        public string Name { get; set; }

        public StatisticResolveAttribute(string name)
        {
            Name = name;
        }
    }
}
