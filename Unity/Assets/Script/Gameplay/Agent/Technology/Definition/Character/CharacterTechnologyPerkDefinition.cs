using UnityEngine;

namespace Game
{
    public abstract class CharacterTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private AgentObjectDefinition affected;
        [SerializeField] private ModifierDefinition modifierDefinition;

        public bool Affect(AgentObjectDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        public void Modify(Agent agent, Entity entity)
        {
            ModifierApplier modifierApplier = agent.GetComponent<ModifierApplier>();
            ModifierHandler target = entity.GetComponent<ModifierHandler>();
            Modifier modifier = modifierDefinition.Instantiate();

            modifierApplier.Apply(modifier, target);
        }
    }
}
