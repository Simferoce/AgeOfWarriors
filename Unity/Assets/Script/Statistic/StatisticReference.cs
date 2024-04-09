using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public string Path { get => path; set => path = value; }

        public StatisticReferenceMapper GetMapper(object caller)
        {
            string[] split = path.ToLower().Split(".");
            string current = split[0];
            string next = split.Length > 0 ? path.Substring(current.Length + 1) : string.Empty;

            switch (current)
            {
                case "ability":
                    return new AbilityReferenceMapper(caller as Ability, next);
                case "character":
                    Character character = caller as Character;
                    if (character == null)
                        character = (caller as Ability)?.Character;
                    return new CharacterReferenceMapper(character, next);
            }

            throw new NotImplementedException();
        }

        public abstract class StatisticReferenceMapper
        {
            public abstract StatisticDefinition GetDefinition();
            public abstract T GetValue();
        }

        public abstract class StatisticReferenceMapper<O> : StatisticReferenceMapper
        {
            protected O context;
            protected string path;

            public StatisticReferenceMapper(O context, string path)
            {
                this.context = context;
                this.path = path;
            }
        }

        public class CharacterReferenceMapper : StatisticReferenceMapper<Character>
        {
            public CharacterReferenceMapper(Character context, string path) : base(context, path)
            {
            }

            public override StatisticDefinition GetDefinition()
                => path switch
                {
                    "maxhealth" => StatisticDefinition.MaxHealth,
                    "defense" => StatisticDefinition.Defense,
                    "attackpower" => StatisticDefinition.AttackPower,
                    "attackspeed" => StatisticDefinition.AttackSpeed,
                    "speed" => StatisticDefinition.Speed,
                    "reach" => StatisticDefinition.Reach,
                    _ => throw new NotImplementedException()
                };

            public override T GetValue()
                => path switch
                {
                    "maxhealth" => (T)(object)context.MaxHealth,
                    "defense" => (T)(object)context.Defense,
                    "attackpower" => (T)(object)context.AttackPower,
                    "attackspeed" => (T)(object)context.AttackSpeed,
                    "speed" => (T)(object)context.Speed,
                    "reach" => (T)(object)context.Reach,
                    _ => throw new NotImplementedException()
                };
        }

        public class AbilityReferenceMapper : StatisticReferenceMapper<Ability>
        {
            public AbilityReferenceMapper(Ability context, string path) : base(context, path)
            {
            }

            public override StatisticDefinition GetDefinition()
            {
                return context.Definition.GetStatistic(path).Definition;
            }

            public override T GetValue()
            {
                return (context.Definition.GetStatistic(path) as Statistic<T>).GetValue(context);
            }
        }
    }
}
