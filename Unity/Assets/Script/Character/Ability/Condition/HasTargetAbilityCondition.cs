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
        [SerializeField, Range(0, 3)] private float reachPercentage = 1f;

        public List<IAttackable> Targets = new List<IAttackable>();

        public override bool Execute()
        {
            Targets = character.GetTargets(criteria, character.Reach * reachPercentage);
            return Targets.Count >= count;
        }
    }
}
