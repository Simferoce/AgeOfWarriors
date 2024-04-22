using System.Reflection;
using UnityEngine;

namespace Game
{
    public abstract class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private GameObject prefab;

        public string Title { get => title; }

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

                if (!typeof(Ability).IsAssignableFrom(parameterInfos[0].ParameterType))
                    continue;

                description = description.Replace($"{{val:{attribute.Name}}}",
                    attribute.HasDescriptor ?
                    attribute.Description(this, caller) :
                    methodInfo.Invoke(this, new object[] { caller }).ToString());
            }

            return description;
        }

        public Ability GetAbility()
        {
            GameObject gameObject = Instantiate(prefab);
            Ability ability = gameObject.GetComponent<Ability>();
            ability.Definition = this;
            return ability;
        }
    }
}
