using System.Reflection;
using UnityEngine;

namespace Game
{
    public abstract class ModifierDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private bool showOnHealthBar = true;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public string Description { get => description; }
        public bool Show { get => showOnHealthBar; set => showOnHealthBar = value; }

        public string ParseDescription(object caller)
        {
            return ParseDescription(caller, this.description);
        }

        public virtual string ParseDescription(object caller, string description)
        {
            MethodInfo[] methodInfos = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                StatisticAttribute attribute = methodInfo.GetCustomAttribute<StatisticAttribute>(true);
                if (attribute == null)
                    continue;

                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length != 1)
                    continue;

                if (!typeof(Modifier).IsAssignableFrom(parameterInfos[0].ParameterType))
                    continue;

                description = description.Replace($"{{val:{attribute.Name}}}",
                    attribute.HasDescriptor ?
                    attribute.Description(this, caller as Modifier) :
                    methodInfo.Invoke(this, new object[] { (caller as Modifier) }).ToString());
            }

            return description;
        }
    }
}