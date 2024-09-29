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

        public override bool Execute(Entity source, Entity targetEntity)
        {
            return (source is AgentObject agentObject) && agentObject.Types.All(x => !types.Contains(x));
        }
    }
}