using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game
{
    [Serializable]
    [MovedFrom(false, "Game", null, "RequirementOr")]
    public class TechnologyRequirementOr : TechnologyRequirement
    {
        [SerializeReference, SubclassSelector] private List<TechnologyRequirement> requirements;

        public override bool Execute(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            return requirements.Any(x => x.Execute(technologyPerkDefinition, technologyTree));
        }

        public override string Format(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            return string.Join(" OR ", requirements.Select(x => x.Format(technologyPerkDefinition, technologyTree)));
        }
    }
}
