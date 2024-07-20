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
    public class AbilityMapper<T> : Mapper<T>
    {
        [SerializeField] private string statistic;

        public override bool TryGetValue(object context, StatisticType type, out T value)
        {
            Ability ability = context as Ability;

            if (ability == null)
                ability = (context as Projectile).Ability;

            if (ability == null)
            {
                value = default(T);
                return false;
            }

            PropertyInfo[] propertyInfos = ability.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                StatisticAttribute statisticAttribute = propertyInfo.GetCustomAttribute<StatisticAttribute>(true);
                if (statisticAttribute == null)
                    continue;

                if (statisticAttribute.Name != statistic)
                    continue;

                value = (T)propertyInfo.GetValue(ability);
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
        [SerializeReference, SubclassSelector] private M mapper;

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
            if (TryGetValue(caller, out T value, type))
                return value;

            throw new Exception($"Could not resolve the statistic for {mapper.ToString()} with {caller.ToString()}");
        }
    }
}
