using System;

namespace Game
{
    public class StatisticAttribute : Attribute
    {
        public string Name { get; set; }

        public StatisticAttribute()
            : this("")
        {

        }

        public StatisticAttribute(string name)
        {
            Name = name;
        }
    }
}
