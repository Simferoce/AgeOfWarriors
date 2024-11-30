using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SlowEnemiesInPoolPerk", menuName = "Definition/Technology/Seer/SlowEnemiesInPoolPerk")]
    public class SlowEnemiesInPoolPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SlowEnemiesInPoolPerk>
        {
            private class Instancier : ApplyPeriodicBuffPoolEffect.Instancier
            {
                private SlowEnemiesInPoolPerk slowEnemiesInPoolPerk;

                public override ModifierDefinition ModifierDefinition => slowEnemiesInPoolPerk.speedModifierDefinition;

                public Instancier(SlowEnemiesInPoolPerk slowEnemiesInPoolPerk)
                {
                    this.slowEnemiesInPoolPerk = slowEnemiesInPoolPerk;
                }

                public override Game.Modifier Instanciate(ModifierHandler modifiable, ModifierApplier modifierSource)
                {
                    return new SpeedModifierDefinition.Modifier(slowEnemiesInPoolPerk.speedModifierDefinition, -slowEnemiesInPoolPerk.amount);
                }

                public override bool Applicable(Pool pool, Target targeteable)
                {
                    if (pool.Faction == (targeteable.Entity as AgentObject).Faction)
                        return false;

                    return true;
                }
            }

            private Character character;

            public Modifier(SlowEnemiesInPoolPerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);

                character = modifiable.Entity.GetCachedComponent<Character>();
                //character.AddOrGetCachedComponent<Ownership>().OnChildAdded += Character_OnChildEntitySpawned;
            }

            private void Character_OnChildEntitySpawned(CachedMonobehaviour entity)
            {
                if (entity.TryGetCachedComponent<Pool>(out Pool pool))
                {
                    pool.AddPoolEffect(new ApplyPeriodicBuffPoolEffect()
                    {
                        ModifierInstancier = new Instancier(definition)
                    });
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                // character.AddOrGetCachedComponent<Ownership>().OnChildAdded -= Character_OnChildEntitySpawned;
            }
        }

        [SerializeField] private SpeedModifierDefinition speedModifierDefinition;
        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount);
        }
    }
}
