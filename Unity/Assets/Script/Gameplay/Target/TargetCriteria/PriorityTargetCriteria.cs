using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PriorityTargetCriteria : TargetCriteria
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

        public override bool Execute(Target owner, Target targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return comparaison switch
            {
                Comparaison.Equal => (targeteable.Entity as AgentObject).Priority == (owner.Entity as AgentObject).Priority,
                Comparaison.Greater => (targeteable.Entity as AgentObject).Priority > (owner.Entity as AgentObject).Priority,
                Comparaison.GreaterOrEqual => (targeteable.Entity as AgentObject).Priority >= (owner.Entity as AgentObject).Priority,
                Comparaison.Lower => (targeteable.Entity as AgentObject).Priority < (owner.Entity as AgentObject).Priority,
                Comparaison.LowerOrEqual => (targeteable.Entity as AgentObject).Priority <= (owner.Entity as AgentObject).Priority,
                _ => throw new NotImplementedException()
            };
        }
    }
}
