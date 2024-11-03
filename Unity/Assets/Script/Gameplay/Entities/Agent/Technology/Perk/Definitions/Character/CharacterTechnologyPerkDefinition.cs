using Game.Character;
using Game.Modifier;
using UnityEngine;

namespace Game.Technology
{
    public abstract class CharacterTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private CharacterDefinition affected;
        [SerializeField] private ModifierDefinition modifierDefinition;

        public bool Affect(CharacterDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        public void Modify(Agent.AgentEntity agent, Entity entity)
        {
            ModifierApplier modifierApplier = agent.GetComponent<ModifierApplier>();
            ModifierHandler target = entity.GetComponent<ModifierHandler>();

            modifierApplier.Apply(modifierDefinition, target);
        }
    }
}
