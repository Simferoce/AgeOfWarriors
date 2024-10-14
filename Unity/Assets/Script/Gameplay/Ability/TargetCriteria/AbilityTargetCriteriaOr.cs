using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaOr : AbilityTargetCriteria
    {
        [SerializeReference, SubclassSelector] private List<AbilityTargetCriteria> criterias = new List<AbilityTargetCriteria>();

        public override bool Execute(Ability source, Entity targetEntity)
        {
            if (criterias.Count == 0)
                return true;

            foreach (AbilityTargetCriteria criteria in criterias)
            {
                if (criteria.Execute(source, targetEntity))
                    return true;
            }

            return false;
        }
    }
}
