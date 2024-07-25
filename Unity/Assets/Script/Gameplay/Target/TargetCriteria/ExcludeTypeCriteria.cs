using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ExcludeTypeCriteria : TargetCriteria
    {
        [SerializeField] private List<AgentObject.Type> types = new List<AgentObject.Type>();

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller, Faction ownerFaction, Faction targetFaction)
        {
            return (targeteable.TryGetCachedComponent<AgentObject>(out AgentObject agentObject)) && agentObject.Types.All(x => !types.Contains(x));
        }
    }
}