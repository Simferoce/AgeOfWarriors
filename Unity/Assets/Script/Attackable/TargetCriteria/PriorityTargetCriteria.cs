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

        public override bool Execute(ITargeteable owner, ITargeteable targeteable)
        {
            return comparaison switch
            {
                Comparaison.Equal => targeteable.Priority == owner.Priority,
                Comparaison.Greater => targeteable.Priority > owner.Priority,
                Comparaison.GreaterOrEqual => targeteable.Priority >= owner.Priority,
                Comparaison.Lower => targeteable.Priority < owner.Priority,
                Comparaison.LowerOrEqual => targeteable.Priority <= owner.Priority,
                _ => throw new NotImplementedException()
            };
        }
    }
}
