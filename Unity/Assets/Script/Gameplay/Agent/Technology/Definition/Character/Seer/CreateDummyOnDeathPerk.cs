﻿using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CreateDummyOnDeathPerk", menuName = "Definition/Technology/Seer/CreateDummyOnDeathPerk")]
    public class CreateDummyOnDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, CreateDummyOnDeathPerk>
        {
            private Character character;

            public Modifier(IModifiable modifiable, CreateDummyOnDeathPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                character = modifiable.GetCachedComponent<Character>();
                character.OnDeath += Character_OnDeath;
            }

            private void Character_OnDeath()
            {
                CharacterDefinition characterDefinition = Resources.Load<CharacterDefinition>("Definition/Character/Dummy/Dummy");
                AgentObject agentObject = characterDefinition.Spawn(character.Agent, character.transform.position, character.SpawnNumber, character.Direction);
                Ownership.SetOwner(agentObject, character);
                IModifiable dummyModifiable = agentObject.GetCachedComponent<IModifiable>();

                float maxHealth = (agentObject as Character).MaxHealth;
                dummyModifiable.AddModifier(new DamageOverTimeModifierDefinition.Modifier(dummyModifiable, definition.damageOverTimeModifierDefinition, modifiable.GetCachedComponent<IModifierSource>(), 9999, maxHealth * definition.percentageHealth));
            }

            public override void Dispose()
            {
                base.Dispose();
                character.OnDeath -= Character_OnDeath;
            }
        }

        [SerializeField] private DamageOverTimeModifierDefinition damageOverTimeModifierDefinition;
        [SerializeField, Range(0, 1)] private float percentageHealth;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, percentageHealth);
        }
    }
}
