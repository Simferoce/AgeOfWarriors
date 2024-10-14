using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaExclude : AbilityTargetCriteria
    {
        [SerializeField] private List<AgentObject.Type> types = new List<AgentObject.Type>();

        public override bool Execute(Ability source, Entity targetEntity)
        {
            return (source.Caster.Entity is AgentObject agentObject) && agentObject.Types.All(x => !types.Contains(x));
        }
    }
}