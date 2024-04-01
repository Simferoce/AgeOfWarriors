using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AndTargetCriteria : TargetCriteria
    {
        [SerializeReference, SubclassSelector] private List<TargetCriteria> criterias = new List<TargetCriteria>();

        public override bool Execute(ITargeteable owner, ITargeteable targeteable)
        {
            foreach (TargetCriteria criteria in criterias)
            {
                if (!criteria.Execute(owner, targeteable))
                    return false;
            }

            return true;
        }
    }
}
