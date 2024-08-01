using System;
using UnityEngine.Scripting.APIUpdating;

namespace Game
{
    [Serializable]
    [MovedFrom(false, "Game", null, "Requirement")]
    public abstract class TechnologyRequirement
    {
        public abstract bool Execute(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree);

        public abstract string Format(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree);
    }
}
