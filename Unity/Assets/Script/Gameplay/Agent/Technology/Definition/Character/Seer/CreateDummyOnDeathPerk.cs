using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CreateDummyOnDeathPerk", menuName = "Definition/Technology/Seer/CreateDummyOnDeathPerk")]
    public class CreateDummyOnDeathPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, CreateDummyOnDeathPerk>
        {
            private Character character;

            public Modifier(CreateDummyOnDeathPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                character = modifiable.Entity.GetCachedComponent<Character>();
                character.OnDeath += Character_OnDeath;
            }

            private void Character_OnDeath()
            {
                CharacterDefinition characterDefinition = Resources.Load<CharacterDefinition>("Definition/Character/Dummy/Dummy");
                AgentObject agentObject = characterDefinition.Spawn(character.Agent, character.transform.position, character.SpawnNumber, character.Direction);
                ModifierHandler dummyModifiable = agentObject.GetCachedComponent<ModifierHandler>();

                float maxHealth = (agentObject as Character).MaxHealth;
                Source.Apply(dummyModifiable, new DamageOverTimeModifierDefinition.Modifier(definition.damageOverTimeModifierDefinition, 9999, maxHealth * definition.percentageHealth));
            }

            public override void Dispose()
            {
                base.Dispose();
                character.OnDeath -= Character_OnDeath;
            }
        }

        [SerializeField] private DamageOverTimeModifierDefinition damageOverTimeModifierDefinition;
        [SerializeField, Range(0, 1)] private float percentageHealth;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, percentageHealth);
        }
    }
}
