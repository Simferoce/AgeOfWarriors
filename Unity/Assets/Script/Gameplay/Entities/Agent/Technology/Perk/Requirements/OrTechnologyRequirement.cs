using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Technology
{
    [Serializable]
    public class OrTechnologyRequirement : TechnologyRequirement
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
