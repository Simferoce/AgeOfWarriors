using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class MapperFactory
    {
        public abstract StatisticReferenceMapper<T> GetMapper<T>(object context);
    }

    [Serializable]
    public class CharacterMapperFactory : MapperFactory
    {
        public enum Statistic
        {
            MaxHealth,
            Defense,
            AttackPower,
            AttackSpeed,
            Reach,
            Speed
        }

        public class CharacterReferenceMapper<T> : StatisticReferenceMapper<Character, T>
        {
            private Statistic statistic;

            public CharacterReferenceMapper(Character context, Statistic statistic) : base(context)
            {
                this.statistic = statistic;
            }

            public override StatisticDefinition GetDefinition()
                => statistic switch
                {
                    Statistic.MaxHealth => StatisticDefinition.MaxHealth,
                    Statistic.Defense => StatisticDefinition.Defense,
                    Statistic.AttackPower => StatisticDefinition.AttackPower,
                    Statistic.AttackSpeed => StatisticDefinition.AttackSpeed,
                    Statistic.Speed => StatisticDefinition.Speed,
                    Statistic.Reach => StatisticDefinition.Reach,
                    _ => throw new NotImplementedException()
                };

            public override T GetValue()
                => statistic switch
                {
                    Statistic.MaxHealth => (T)(object)context.MaxHealth,
                    Statistic.Defense => (T)(object)context.Defense,
                    Statistic.AttackPower => (T)(object)context.AttackPower,
                    Statistic.AttackSpeed => (T)(object)context.AttackSpeed,
                    Statistic.Speed => (T)(object)context.Speed,
                    Statistic.Reach => (T)(object)context.Reach,
                    _ => throw new NotImplementedException()
                };
        }

        [SerializeField] private Statistic statistic;

        public override StatisticReferenceMapper<T> GetMapper<T>(object context)
        {
            Character character = context as Character;
            if (character == null)
                character = (context as Ability)?.Character;

            return new CharacterReferenceMapper<T>(character, statistic);
        }
    }

    [Serializable]
    public class AbilityMapperFactory : MapperFactory
    {
        public class AbilityReferenceMapper<T> : StatisticReferenceMapper<Ability, T>
        {
            private string path;

            public AbilityReferenceMapper(Ability context, string path) : base(context)
            {
                this.path = path;
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

        [SerializeField] private string statistic;

        public override StatisticReferenceMapper<T> GetMapper<T>(object context)
        {
            return new AbilityReferenceMapper<T>(context as Ability, statistic);
        }
    }

    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeReference, SerializeReferenceDropdown] private MapperFactory mapperFactory;

        public StatisticReferenceMapper<T> GetMapper(object caller)
        {
            return mapperFactory?.GetMapper<T>(caller);
        }
    }

    public abstract class StatisticReferenceMapper
    {
        public abstract StatisticDefinition GetDefinition();
    }

    public abstract class StatisticReferenceMapper<T> : StatisticReferenceMapper
    {
        public abstract T GetValue();
    }

    public abstract class StatisticReferenceMapper<O, T> : StatisticReferenceMapper<T>
    {
        protected O context;

        public StatisticReferenceMapper(O context)
        {
            this.context = context;
        }
    }
}
