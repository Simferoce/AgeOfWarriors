using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AndTargetCriteria : TargetCriteria
    {
        [SerializeReference, SubclassSelector] private List<TargetCriteria> criterias = new List<TargetCriteria>();

        public override bool Execute(Entity source, Entity targetEntity)
        {
            foreach (TargetCriteria criteria in criterias)
            {
                if (!criteria.Execute(source, targetEntity))
                    return false;
            }

            return true;
        }
    }
}
