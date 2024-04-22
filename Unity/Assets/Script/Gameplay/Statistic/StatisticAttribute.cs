using System;
using System.Reflection;

namespace Game
{
    public class StatisticAttribute : Attribute
    {
        public string Name { get; set; }
        public string DescriptorFunction { get; set; }

        public bool HasDescriptor => DescriptorFunction != null;

        public StatisticAttribute(string name, string descriptorFunction = null)
        {
            Name = name;
            DescriptorFunction = descriptorFunction;
        }

        public string Description(object instance)
        {
            MethodInfo methodInfo = instance.GetType().GetMethod(DescriptorFunction);
            return (string)methodInfo.Invoke(instance, null);
        }
    }
}
