using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public Statistic<T> Get(StatisticContext context)
        {
            string[] node = path.Split('.');
            StatisticContextElement current = context.Children[node[0]];
            for (int i = 1; i < node.Length; ++i)
            {
                current = current.Children[node[i]];
            }

            return (Statistic<T>)current.Value;
        }

        public StatisticReference<T> Clone()
        {
            StatisticReference<T> statisticReference = new StatisticReference<T>();
            statisticReference.path = path;
            return statisticReference;
        }
    }

    public class StatisticContext
    {
        public Dictionary<string, StatisticContextElement> Children { get; set; }

        public StatisticContext(params (string key, StatisticContextElement context)[] children)
        {
            Children = new Dictionary<string, StatisticContextElement>(children.ToDictionary(x => x.key, x => x.context));
        }
    }

    public class StatisticContextElement
    {
        public string Path { get; set; }
        public object Value { get; set; }

        public Dictionary<string, StatisticContextElement> Children { get; set; }

        public StatisticContextElement(string path, object value) : this(path, value, new Dictionary<string, StatisticContextElement>())
        {
        }

        public StatisticContextElement(string path, object value, Dictionary<string, StatisticContextElement> children)
        {
            Children = children;
            Value = value;
            Path = path;
        }
    }

    public class AbilityStatisticContext : StatisticContextElement
    {
        public AbilityStatisticContext(CharacterAbility value)
            : base("ability", value,
                  new Dictionary<string, StatisticContextElement>() {
                      { "range", new StatisticContextElement("range", value.Range) },
                      { "base", new AbilityDefinitionContext(value.Definition) },
                      { "caster", new CharacterStatisticContext(value.Character) } })
        {

        }
    }

    public class AbilityDefinitionContext : StatisticContextElement
    {
        public AbilityDefinitionContext(AbilityDefinition definition)
            : base("definition", definition, new Dictionary<string, StatisticContextElement>()
            {
                { "range", new StatisticContextElement("range", definition.Range) }
            })
        {
        }
    }

    public class CharacterStatisticContext : StatisticContextElement
    {
        public CharacterStatisticContext(Character character) : base("character", character,
            new Dictionary<string, StatisticContextElement>() { { "reach", new StatisticContextElement("reach", character.Reach) } })
        {
        }
    }

    public class TargeableStatisticContext : StatisticContextElement
    {
        public TargeableStatisticContext(ITargeteable targeteable) : base("targeteable", targeteable,
            new Dictionary<string, StatisticContextElement>() { { "reach", new StatisticContextElement("reach", (targeteable as Character)?.Reach) } })
        {
        }
    }
}
