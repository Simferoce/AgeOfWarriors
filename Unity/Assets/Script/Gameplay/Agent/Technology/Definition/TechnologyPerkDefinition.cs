using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class TechnologyPerkDefinition : ModifierDefinition
    {
        [Space]
        [SerializeReference, SubclassSelector] private List<Requirement> requirementsPerk = new List<Requirement>();

        public List<Requirement> RequirementsPerk { get => requirementsPerk; set => requirementsPerk = value; }

        public bool IsUnlockable(Agent agent)
        {
            foreach (Requirement requirement in requirementsPerk)
            {
                if (!requirement.Execute(agent))
                    return false;
            }

            return true;
        }
    }
}