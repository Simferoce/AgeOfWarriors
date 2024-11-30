using Assets.Script.Agent.Technology;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterTechnologyPerkDefinition : TechnologyPerkDefinition, ITechnologyModify
    {
        [SerializeField] private AgentObjectDefinition affected;
        [SerializeField] private ModifierDefinition modifierDefinition;

        public class Modifier : Modifier<Modifier, CharacterTechnologyPerkDefinition>
        {
            private Agent agent;

            public Modifier(CharacterTechnologyPerkDefinition modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                if (modifiable.Entity is not Agent agent)
                {
                    Debug.LogError($"Expecting the entity of the modifiable to be {nameof(Agent)} but got {modifiable.GetType()} instead.");
                    return;
                }

                this.agent = agent;
                agent.OnEntityCreated += AgentOnEntityCreated;
            }

            private void AgentOnEntityCreated(Entity entity)
            {
                modifiable.Entity.GetCachedComponent<ModifierApplier>().Apply(entity.GetCachedComponent<ModifierHandler>(), definition.modifierDefinition.Instantiate(), new List<ModifierParameter>());
            }

            public override void Dispose()
            {
                base.Dispose();

                agent.OnEntityCreated -= AgentOnEntityCreated;
            }

            private void EntityCreated(EntityCreatedEventChannel.Event evt)
            {

            }
        }

        public bool Affect(AgentObjectDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
