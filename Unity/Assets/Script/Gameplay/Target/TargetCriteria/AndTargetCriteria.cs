using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AndTargetCriteria : TargetCriteria
    {
        [SerializeReference, SubclassSelector] private List<TargetCriteria> criterias = new List<TargetCriteria>();

        public override bool Execute(Target owner, Target targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            foreach (TargetCriteria criteria in criterias)
            {
                if (!criteria.Execute(owner, targeteable, statisticProvider, ownerFaction, targetFaction))
                    return false;
            }

            return true;
        }
    }
}
