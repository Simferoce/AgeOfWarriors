using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CreateAttackBuffingPoolOnDeath", menuName = "Definition/Technology/Seer/CreateAttackBuffingPoolOnDeath")]
    public class CreateAttackBuffingPoolOnDeath : CharacterTechnologyPerkDefinition
    {
        private class ModifierInstancier : ApplyPeriodicBuffPoolEffect.Instancier
        {
            private CreateAttackBuffingPoolOnDeath createAttackBuffingPoolOnDeath;

            public ModifierInstancier(CreateAttackBuffingPoolOnDeath createAttackBuffingPoolOnDeath)
            {
                this.createAttackBuffingPoolOnDeath = createAttackBuffingPoolOnDeath;
            }

            public override ModifierDefinition ModifierDefinition => createAttackBuffingPoolOnDeath.modifierDefinition;

            public override Game.Modifier Instanciate(IModifiable modifiable, IModifierSource modifierSource)
            {
                return new AttackPowerModifierDefinition.AttackPowerModifier(modifiable, createAttackBuffingPoolOnDeath.modifierDefinition, createAttackBuffingPoolOnDeath.amount, modifierSource);
            }
        }

        private class Modifier : Modifier<Modifier, CreateAttackBuffingPoolOnDeath>
        {
            private Character character;

            public Modifier(IModifiable modifiable, CreateAttackBuffingPoolOnDeath modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                character = modifiable.GetCachedComponent<Character>();
                character.OnDeath += Modifier_OnDeath;
            }

            private void Modifier_OnDeath()
            {
                Vector3 position = Lane.Instance.Project(character.transform.position);
                Pool pool = GameObject.Instantiate(definition.poolPrefab, position, Quaternion.identity).GetComponent<Pool>();
                pool.Duration = definition.duration;
                pool.GetEffect<ApplyPeriodicBuffPoolEffect>().ModifierInstancier = new ModifierInstancier(this.definition);
                pool.Spawn(character.Agent, 0, character.Agent.Direction);
                pool.Initialize(modifiable.GetCachedComponent<AgentObject>());
            }

            public override void Dispose()
            {
                base.Dispose();
                character.OnDeath -= Modifier_OnDeath;
            }
        }

        [SerializeField] private AttackPowerModifierDefinition modifierDefinition;
        [SerializeField] private GameObject poolPrefab;
        [SerializeField] private float duration;
        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, duration, amount);
        }
    }
}