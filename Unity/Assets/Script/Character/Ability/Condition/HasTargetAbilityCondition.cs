using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HasTargetAbilityCondition : AbilityCondition
    {
        [SerializeReference, SubclassSelector] private TargetCriteria criteria;
        [SerializeField] private int count = 1;

        public List<IAttackable> Targets = new List<IAttackable>();

        public override bool Execute()
        {
            Targets = character.GetTargets(criteria);
            return Targets.Count >= count;
        }

        public override AbilityCondition Clone()
        {
            HasTargetAbilityCondition hasTargetAbilityCondition = new HasTargetAbilityCondition();
            hasTargetAbilityCondition.criteria = criteria.Clone();
            hasTargetAbilityCondition.count = count;

            return hasTargetAbilityCondition;
        }
    }
}
