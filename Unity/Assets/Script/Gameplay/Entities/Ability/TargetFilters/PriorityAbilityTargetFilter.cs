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
                Comparaison.Equal => (targetEntity as AgentObject).Priority == (source.Caster.Entity as AgentObject).Priority,
                Comparaison.Greater => (targetEntity as AgentObject).Priority > (source.Caster.Entity as AgentObject).Priority,
                Comparaison.GreaterOrEqual => (targetEntity as AgentObject).Priority >= (source.Caster.Entity as AgentObject).Priority,
                Comparaison.Lower => (targetEntity as AgentObject).Priority < (source.Caster.Entity as AgentObject).Priority,
                Comparaison.LowerOrEqual => (targetEntity as AgentObject).Priority <= (source.Caster.Entity as AgentObject).Priority,
                _ => throw new NotImplementedException()
            };
        }
    }
}
