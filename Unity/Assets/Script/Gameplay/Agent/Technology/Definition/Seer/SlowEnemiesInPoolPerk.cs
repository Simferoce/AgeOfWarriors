using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SlowEnemiesInPoolPerk", menuName = "Definition/Technology/Seer/SlowEnemiesInPoolPerk")]
    public class SlowEnemiesInPoolPerk : CharacterTechnologyPerkDefinition
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

                public override Game.Modifier Instanciate(IModifiable modifiable, IModifierSource modifierSource)
                {
                    return new SpeedModifierDefinition.Modifier(modifiable, slowEnemiesInPoolPerk.speedModifierDefinition, -slowEnemiesInPoolPerk.amount, modifierSource);
                }

                public override bool Applicable(Pool pool, ITargeteable targeteable)
                {
                    if (pool.Faction == targeteable.Faction)
                        return false;

                    return true;
                }
            }

            private Character character;

            public Modifier(IModifiable modifiable, SlowEnemiesInPoolPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                character = modifiable.GetCachedComponent<Character>();
                character.OnChildEntitySpawned += Character_OnChildEntitySpawned;
            }

            private void Character_OnChildEntitySpawned(Entity entity)
            {
                if (entity is Pool pool)
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
                character.OnChildEntitySpawned -= Character_OnChildEntitySpawned;
            }
        }

        [SerializeField] private SpeedModifierDefinition speedModifierDefinition;
        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount);
        }
    }
}
