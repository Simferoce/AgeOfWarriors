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

        public List<Target> Targets { get; set; } = new List<Target>();

        public override bool Execute()
        {
            Targets.Clear();
            Targets.AddRange(TargetUtility.GetTargets(ability.Caster.Entity, criteria, ability));
            return Targets.Count >= count;
        }
    }
}
