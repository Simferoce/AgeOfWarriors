using Game.Agent;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class PriorityAbilityTargetFilter : AbilityTargetFilter
    {
        private enum Comparaison
        {
            Equal,
            Greater,
            GreaterOrEqual,
            Lower,
            LowerOrEqual
        }

        [SerializeField] private Comparaison comparaison;

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return comparaison switch
            {
                Comparaison.Equal => targetEntity.GetCachedComponent<AgentIdentity>().Priority == source.Caster.Entity.GetCachedComponent<AgentIdentity>().Priority,
                Comparaison.Greater => targetEntity.GetCachedComponent<AgentIdentity>().Priority > source.Caster.Entity.GetCachedComponent<AgentIdentity>().Priority,
                Comparaison.GreaterOrEqual => targetEntity.GetCachedComponent<AgentIdentity>().Priority >= source.Caster.Entity.GetCachedComponent<AgentIdentity>().Priority,
                Comparaison.Lower => targetEntity.GetCachedComponent<AgentIdentity>().Priority < source.Caster.Entity.GetCachedComponent<AgentIdentity>().Priority,
                Comparaison.LowerOrEqual => targetEntity.GetCachedComponent<AgentIdentity>().Priority <= source.Caster.Entity.GetCachedComponent<AgentIdentity>().Priority,
                _ => throw new NotImplementedException()
            };
        }
    }
}
