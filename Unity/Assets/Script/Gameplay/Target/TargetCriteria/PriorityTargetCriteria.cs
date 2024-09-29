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

        public override bool Execute(Entity source, Entity targetEntity)
        {
            return comparaison switch
            {
                Comparaison.Equal => (targetEntity as AgentObject).Priority == (source as AgentObject).Priority,
                Comparaison.Greater => (targetEntity as AgentObject).Priority > (source as AgentObject).Priority,
                Comparaison.GreaterOrEqual => (targetEntity as AgentObject).Priority >= (source as AgentObject).Priority,
                Comparaison.Lower => (targetEntity as AgentObject).Priority < (source as AgentObject).Priority,
                Comparaison.LowerOrEqual => (targetEntity as AgentObject).Priority <= (source as AgentObject).Priority,
                _ => throw new NotImplementedException()
            };
        }
    }
}
