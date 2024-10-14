using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaAnd : AbilityTargetCriteria
    {
        [SerializeReference, SubclassSelector] private List<AbilityTargetCriteria> criterias = new List<AbilityTargetCriteria>();

        public override bool Execute(Ability source, Entity targetEntity)
        {
            foreach (AbilityTargetCriteria criteria in criterias)
            {
                if (!criteria.Execute(source, targetEntity))
                    return false;
            }

            return true;
        }
    }
}
