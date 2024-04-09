using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AndTargetCriteria : TargetCriteria
    {
        [SerializeReference, SerializeReferenceDropdown] private List<TargetCriteria> criterias = new List<TargetCriteria>();

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, StatisticContext context)
        {
            foreach (TargetCriteria criteria in criterias)
            {
                if (!criteria.Execute(owner, targeteable, context))
                    return false;
            }

            return true;
        }
    }
}
