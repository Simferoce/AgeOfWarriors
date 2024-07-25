using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PerkRequirement : Requirement
    {
        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;

        public TechnologyPerkDefinition TechnologyPerkDefinition { get => technologyPerkDefinition; set => technologyPerkDefinition = value; }

        public override bool Execute(Agent agent)
        {
            return technologyPerkDefinition.IsUnlocked(agent);
        }
    }
}
