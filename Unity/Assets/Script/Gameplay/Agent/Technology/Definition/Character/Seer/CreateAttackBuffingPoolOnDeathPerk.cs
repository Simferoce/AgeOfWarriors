using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CreateAttackBuffingPoolOnDeathPerk", menuName = "Definition/Technology/Seer/CreateAttackBuffingPoolOnDeathPerk")]
    public class CreateAttackBuffingPoolOnDeathPerk : CharacterTechnologyPerkDefinition
    {
        private class ModifierInstancier : ApplyPeriodicBuffPoolEffect.Instancier
        {
            private CreateAttackBuffingPoolOnDeathPerk createAttackBuffingPoolOnDeath;

            public ModifierInstancier(CreateAttackBuffingPoolOnDeathPerk createAttackBuffingPoolOnDeath)
            {
                this.createAttackBuffingPoolOnDeath = createAttackBuffingPoolOnDeath;
            }

            public override ModifierDefinition ModifierDefinition => createAttackBuffingPoolOnDeath.modifierDefinition;

            public override bool Applicable(Pool pool, Target targeteable)
            {
                if ((targeteable.Entity as AgentObject).Faction != pool.Faction)
                    return false;

                return true;
            }

            public override Game.Modifier Instanciate(ModifierHandler modifiable, IModifierSource modifierSource)
            {
                return new AttackPowerModifierDefinition.AttackPowerModifier(modifiable, createAttackBuffingPoolOnDeath.modifierDefinition, createAttackBuffingPoolOnDeath.amount, modifierSource);
            }
        }

        private class Modifier : Modifier<Modifier, CreateAttackBuffingPoolOnDeathPerk>
        {
            private Character character;

            public Modifier(ModifierHandler modifiable, CreateAttackBuffingPoolOnDeathPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                character = modifiable.Entity.GetCachedComponent<Character>();
                character.OnDeath += Modifier_OnDeath;
            }

            private void Modifier_OnDeath()
            {
                Vector3 position = Lane.Instance.Project(character.transform.position);
                Pool pool = GameObject.Instantiate(definition.poolPrefab, position, Quaternion.identity).GetComponent<Pool>();
                pool.Duration = definition.duration;
                pool.GetEffect<ApplyPeriodicBuffPoolEffect>().ModifierInstancier = new ModifierInstancier(this.definition);
                pool.Spawn(character.Agent, 0, character.Agent.Direction);
                //Ownership.SetOwner(pool, character);
                throw new System.Exception();

                pool.Initialize();
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

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, duration, amount);
        }
    }
}