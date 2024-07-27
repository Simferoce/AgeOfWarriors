using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public abstract class TechnologyPerkDefinition : ModifierDefinition
    {
        [Space]
        [FormerlySerializedAs("requirementsPerk")]
        [SerializeReference, SubclassSelector] private List<Requirement> requirements = new List<Requirement>();

        public List<Requirement> Requirements { get => requirements; set => requirements = value; }

        public bool IsUnlockable(Agent agent)
        {
            foreach (Requirement requirement in requirements)
            {
                if (!requirement.Execute(agent))
                    return false;
            }

            return true;
        }

        public bool HasRequirements()
        {
            return requirements.Count > 0;
        }

        public string FormatRequirements(Agent agent)
        {
            return string.Join(" AND ", requirements.Select(x => x.Format(agent)));
        }
    }
}