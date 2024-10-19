using System;

namespace Game.Technology
{
    [Serializable]
    public abstract class TechnologyRequirement
    {
        public abstract bool Execute(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree);

        public abstract string Format(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree);
    }
}
