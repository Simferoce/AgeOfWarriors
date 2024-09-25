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

        public void Modify(Entity entity)
        {
            modifierDefinition.Instantiate(entity.GetCachedComponent<ModifierHandler>(), null);
        }
    }
}
