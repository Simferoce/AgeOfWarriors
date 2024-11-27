using Assets.Script.Agent.Technology;
using UnityEngine;

namespace Game
{
    public abstract class CharacterTechnologyPerkDefinition : TechnologyPerkDefinition, ITechnologyModify
    {
        [SerializeField] private AgentObjectDefinition affected;

        public bool Affect(AgentObjectDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        public abstract Modifier GetModifier(ModifierHandler modifiable);
    }
}
