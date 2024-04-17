using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PerkRequirement : Requirement
    {
        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;

        public override bool Execute(Agent agent)
        {
            return technologyPerkDefinition.IsUnlocked(agent);
        }
    }
}
