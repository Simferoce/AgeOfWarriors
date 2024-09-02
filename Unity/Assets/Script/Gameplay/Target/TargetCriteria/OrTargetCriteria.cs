using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class OrTargetCriteria : TargetCriteria
    {
        [SerializeReference, SubclassSelector] private List<TargetCriteria> criterias = new List<TargetCriteria>();

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IStatisticProviderOld statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            if (criterias.Count == 0)
                return true;

            foreach (TargetCriteria criteria in criterias)
            {
                if (criteria.Execute(owner, targeteable, statisticProvider, ownerFaction, targetFaction))
                    return true;
            }

            return false;
        }
    }
}
