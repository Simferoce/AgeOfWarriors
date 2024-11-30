using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CreateAttackBuffingPoolOnDeathPerk", menuName = "Definition/Technology/Seer/CreateAttackBuffingPoolOnDeathPerk")]
    public class CreateAttackBuffingPoolOnDeathPerk : ModifierDefinition
    {
        private class Modifier : Modifier<Modifier, CreateAttackBuffingPoolOnDeathPerk>
        {
            private Character character;

            public Modifier(CreateAttackBuffingPoolOnDeathPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                character = modifiable.Entity.GetCachedComponent<Character>();
                character.OnDeath += Modifier_OnDeath;
            }

            private void Modifier_OnDeath()
            {
                Vector3 position = Lane.Instance.Project(character.transform.position);
                Pool pool = GameObject.Instantiate(definition.poolPrefab, position, Quaternion.identity).GetComponent<Pool>();
                pool.Duration = definition.duration;
                pool.Spawn(character.Agent, 0, character.Agent.Direction);

                pool.Initialize();
            }

            public override void Dispose()
            {
                base.Dispose();
                character.OnDeath -= Modifier_OnDeath;
            }
        }

        [SerializeField] private StatisticModifierDefinition modifierDefinition;
        [SerializeField] private GameObject poolPrefab;
        [SerializeField] private float duration;
        [SerializeField] private float amount;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, duration, amount);
        }
    }
}