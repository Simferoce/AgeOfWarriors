using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaPriority : AbilityTargetCriteria
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

        public override bool Execute(Ability source, Entity targetEntity)
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
