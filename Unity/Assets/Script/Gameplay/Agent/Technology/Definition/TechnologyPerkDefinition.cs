using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class TechnologyPerkDefinition : ModifierDefinition
    {
        [Space]
        [SerializeReference, SerializeReferenceDropdown] private List<Requirement> requirementsPerk = new List<Requirement>();

        public List<Requirement> RequirementsPerk { get => requirementsPerk; set => requirementsPerk = value; }

        public virtual bool IsUnlocked(Agent agent)
        {
            return agent.Technology.PerksUnlocked.Contains(this);
        }

        public virtual bool IsUnlockable(Agent agent)
        {
            return requirementsPerk.Any(x => x.Execute(agent));
        }
    }
}