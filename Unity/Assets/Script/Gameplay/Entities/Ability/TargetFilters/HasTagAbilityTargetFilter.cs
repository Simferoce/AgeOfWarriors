using Game.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class HasTagAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeField] private List<AgentObject.EntityTag> types = new List<AgentObject.EntityTag>();

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return (targetEntity is AgentObject agentObject) && agentObject.Tags.Any(x => types.Contains(x));
        }
    }
}
