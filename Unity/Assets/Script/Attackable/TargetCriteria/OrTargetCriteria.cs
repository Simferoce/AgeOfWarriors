using System;
using System.Collections.Generic;
using System.Linq;
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

        public override TargetCriteria Clone()
        {
            OrTargetCriteria orTargetCriteria = new OrTargetCriteria();
            orTargetCriteria.criterias = criterias.Select(x => x.Clone()).ToList();

            return orTargetCriteria;
        }
    }
}
