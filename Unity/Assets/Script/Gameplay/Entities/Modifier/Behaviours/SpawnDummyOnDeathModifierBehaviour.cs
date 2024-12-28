using Game.Agent;
using Game.Character;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SpawnDummyOnDeathModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private CharacterDefinition characterDefinition;
        [SerializeField] private ModifierDefinition damageOverTime;
        [SerializeReference, SubclassSelector] private ModifierParameterFactory damage;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifier.Target.Entity.EventChannelHandler.Subscribe<DeathEventChannel.Event>(OnDeath);
            damage.Initialize(modifier);
        }

        private void OnDeath(DeathEventChannel.Event evt)
        {
            AgentIdentity agentIdentity = evt.Entity.GetCachedComponent<AgentIdentity>();
            CharacterEntity characterEntity = agentIdentity.Agent.SpawnAgentObject(characterDefinition, evt.Entity.transform.position, agentIdentity.Direction, agentIdentity.SpawnNumber);
            ModifierHandler modifierHandler = characterEntity.AddOrGetCachedComponent<ModifierHandler>();
            ModifierApplier modifierApplier = modifier.AddOrGetCachedComponent<ModifierApplier>();

            modifierApplier.Apply(damageOverTime, modifierHandler, damage.Create(modifier));
        }

        public override void Dispose()
        {
            base.Dispose();
            modifier.Target.Entity.EventChannelHandler.Unsubscribe<DeathEventChannel.Event>(OnDeath);
        }
    }
}
