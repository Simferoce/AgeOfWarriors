using System;
using System.Reflection;
using UnityEngine;

namespace Game
{
    #region Float Field
    public interface MapperFloat
    {

    }

    [Serializable]
    public class StatisticReference<T> : StatisticReference<T, MapperFloat> { }

    [Serializable]
    public class AbilityMapperFloat : AbilityMapper<float>, MapperFloat { }

    [Serializable]
    public class CharacterMapperFloat : CharacterMapper<float>, MapperFloat { }

    #endregion

    #region Mapper
    [Serializable]
    public abstract class Mapper
    {

    }

    [Serializable]
    public abstract class Mapper<T> : Mapper
    {
        public abstract bool TryGetValue(object context, StatisticType type, out T value);
    }

    [Serializable]
    public class CharacterMapper<T> : Mapper<T>
    {
        [SerializeField] private StatisticDefinition definition;

        public override bool TryGetValue(object context, StatisticType type, out T value)
        {
            Character character = context as Character;
            if (character == null)
                character = (context as Ability)?.Character;
            if (character == null && context is Modifier modifier)
                character = modifier.Modifiable.GetCachedComponent<Character>();

            if (character == null)
            {
                value = default(T);
                return false;
            }

            value = default(T);
            return false;
        }
    }

    [Serializable]
    public class AbilityMapper<T> : Mapper<T>
    {
        [SerializeField] private string statistic;

        public override bool TryGetValue(object context, StatisticType type, out T value)
        {
            Ability ability = context as Ability;

            if (ability == null)
            {
                value = default(T);
                return false;
            }

            MethodInfo[] methodInfos = ability.Definition.GetType().GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                StatisticAttribute statisticAttribute = methodInfo.GetCustomAttribute<StatisticAttribute>(true);
                if (statisticAttribute == null)
                    continue;

                if (statisticAttribute.Name != statistic)
                    continue;

                value = (T)methodInfo.Invoke(ability.Definition, new object[] { ability });
                return true;
            }

            value = default(T);
            return false;
        }

        public override string ToString()
        {
            return $"AbilityMapper ({statistic})";
        }
    }

    #endregion

    public enum StatisticType
    {
        Base,
        Modified,
        Total
    }


    [Serializable]
    public class StatisticReference<T, M>
    {
        [SerializeReference, SerializeReferenceDropdown] private M mapper;

        public bool TryGetValue(object caller, out T value, StatisticType type = StatisticType.Total)
        {
            if (mapper == null)
            {
                value = default(T);
                return false;
            }

            return (mapper as Mapper<T>).TryGetValue(caller, type, out value);
        }

        public T GetValueOrDefault(object caller, StatisticType type = StatisticType.Total)
        {
            return TryGetValue(caller, out T value, type) == true ? value : default(T);
        }

        public T GetValueOrThrow(object caller, StatisticType type = StatisticType.Total)
        {
            return TryGetValue(caller, out T value, type) == true ? value : throw new Exception($"Could not resolve the statistic for {mapper.ToString()} with {caller.ToString()}");
        }
    }
}
