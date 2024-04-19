using System;
using UnityEngine;

namespace Game
{
    #region Float Field
    public interface MapperFloat
    {

    }

    public interface MapperInt
    {

    }

    [Serializable]
    public class StatisticReference<T> : StatisticReference<T, MapperFloat> { }

    [Serializable]
    public class AbilityMapperFloat : AbilityMapper<float>, MapperFloat { }

    [Serializable]
    public class CharacterMapperFloat : CharacterMapper<float>, MapperFloat { }

    [Serializable]
    public class ModifierMapperFloat : ModifierMapper<float>, MapperFloat { }

    [Serializable]
    public class ModifierMapperInt : ModifierMapper<int>, MapperInt { }

    [Serializable]
    public class ModifierDefinitionMapperInt : ModifierDefinitionMapper<int>, MapperInt { }

    [Serializable]
    public class ModifierDefinitionMapperFloat : ModifierDefinitionMapper<float>, MapperFloat { }
    #endregion

    #region Mapper
    [Serializable]
    public abstract class Mapper
    {

    }

    [Serializable]
    public abstract class Mapper<T> : Mapper
    {
        public abstract bool TryGetValue(object context, StatisticDefinition definition, StatisticType type, out T value);
    }

    [Serializable]
    public class CharacterMapper<T> : Mapper<T>
    {
        public override bool TryGetValue(object context, StatisticDefinition definition, StatisticType type, out T value)
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

            return character.TryGetStatisticValue<T>(definition, type, out value);
        }
    }

    [Serializable]
    public class AbilityMapper<T> : Mapper<T>
    {
        public override bool TryGetValue(object context, StatisticDefinition definition, StatisticType type, out T value)
        {
            Ability ability = context as Ability;
            if (ability == null)
                ability = (context as Projectile)?.Ability;

            if (ability == null)
            {
                value = default(T);
                return false;
            }

            return ability.TryGetValue(definition, out value);
        }
    }

    [Serializable]
    public class ModifierMapper<T> : Mapper<T>
    {
        public override bool TryGetValue(object context, StatisticDefinition definition, StatisticType type, out T value)
        {
            Modifier modifier = context as Modifier;

            if (modifier == null)
            {
                value = default(T);
                return false;
            }

            return modifier.TryGetValue(definition, out value);
        }
    }

    [Serializable]
    public class ModifierDefinitionMapper<T> : Mapper<T>
    {
        [SerializeField] private ModifierDefinition modifierDefinition;

        public override bool TryGetValue(object context, StatisticDefinition definition, StatisticType type, out T value)
        {
            return modifierDefinition.TryGetValue<T>(context, definition, out value);
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
        [SerializeField] private StatisticDefinition definition;
        [SerializeReference, SerializeReferenceDropdown] private M mapper;

        public StatisticDefinition Definition { get => definition; set => definition = value; }

        public bool TryGetValue(object caller, out T value, StatisticType type = StatisticType.Total)
        {
            if (mapper == null)
            {
                value = default(T);
                return false;
            }

            return (mapper as Mapper<T>).TryGetValue(caller, Definition, type, out value);
        }

        public T GetValueOrDefault(object caller, StatisticType type = StatisticType.Total)
        {
            return TryGetValue(caller, out T value, type) == true ? value : default(T);
        }

        public T GetValueOrThrow(object caller, StatisticType type = StatisticType.Total)
        {
            return TryGetValue(caller, out T value, type) == true ? value : throw new Exception($"Could not resolve the statistic {definition} for {mapper.GetType().Name} with {caller.ToString()}");
        }

        public override string ToString()
        {
            return $"{{{Definition}}}";
        }
    }
}
