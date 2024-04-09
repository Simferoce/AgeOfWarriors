using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class OrTargetCriteria : TargetCriteria
    {
        [SerializeReference, SerializeReferenceDropdown] private List<TargetCriteria> criterias = new List<TargetCriteria>();

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, StatisticContext context)
        {
            if (criterias.Count == 0)
                return true;

            foreach (TargetCriteria criteria in criterias)
            {
                if (criteria.Execute(owner, targeteable, context))
                    return true;
            }

            return false;
        }
    }
}
