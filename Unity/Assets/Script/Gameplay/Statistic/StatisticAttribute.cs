using System;
using System.Reflection;

namespace Game
{
    public static class StatisticFormatter
    {
        public static string Percentage<T>(Func<T, float> value, float percentage, StatisticDefinition definition, Modifier modifier)
            where T : Modifier
            => modifier != null
            ? $"{value.Invoke(modifier as T)} <color=#{definition.ColorHex}>({percentage.ToString("0.0%")}{definition.TextIcon})</color>"
            : $"<color=#{definition.ColorHex}>({percentage.ToString("0.0%")}{definition.TextIcon})</color>";
    }

    [AttributeUsage(AttributeTargets.Method)]
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

        public string Description(object definition, object instance)
        {
            MethodInfo methodInfo = definition.GetType().GetMethod(DescriptorFunction);
            return (string)methodInfo.Invoke(definition, new object[] { instance });
        }
    }
}
