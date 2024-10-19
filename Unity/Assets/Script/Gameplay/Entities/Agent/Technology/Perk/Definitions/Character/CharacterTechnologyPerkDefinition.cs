using Game.Agent;
using Game.Modifier;
using UnityEngine;

namespace Game.Technology
{
    public abstract class CharacterTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private AgentObjectDefinition affected;
        [SerializeField] private ModifierDefinition modifierDefinition;

        public bool Affect(AgentObjectDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        public void Modify(Agent.AgentEntity agent, Entity entity)
        {
            ModifierApplier modifierApplier = agent.GetComponent<ModifierApplier>();
            ModifierHandler target = entity.GetComponent<ModifierHandler>();
            ModifierEntity modifier = modifierDefinition.Instantiate();

            modifierApplier.Apply(modifier, target);
        }
    }
}
