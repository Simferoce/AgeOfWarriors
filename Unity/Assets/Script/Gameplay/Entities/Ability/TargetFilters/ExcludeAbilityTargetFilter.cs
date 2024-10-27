using Game.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class ExcludeAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeField] private List<AgentObject.EntityTag> types = new List<AgentObject.EntityTag>();

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return (targetEntity is AgentObject agentObject) && agentObject.Tags.All(x => !types.Contains(x));
        }
    }
}