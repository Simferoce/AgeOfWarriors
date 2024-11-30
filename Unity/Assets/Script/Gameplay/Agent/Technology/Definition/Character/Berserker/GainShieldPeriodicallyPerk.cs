using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldPeriodicallyPerk", menuName = "Definition/Technology/Berserker/GainShieldPeriodicallyPerk")]
    public class GainShieldPeriodicallyPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainShieldPeriodicallyPerk>
        {
            private float accumulatedTime = 0.0f;

            public Modifier(GainShieldPeriodicallyPerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Update()
            {
                base.Update();

                if (modifiable.Entity.TryGetCachedComponent<Character>(out Character character) && character.IsActive && character.IsEngaged)
                {
                    if ((int)((accumulatedTime + Time.deltaTime) / definition.cooldown) != (int)(accumulatedTime / definition.cooldown))
                    {
                        Source.Apply(modifiable, definition.shieldModifierDefinition.CreateShield(definition.amount, definition.duration));
                    }

                    accumulatedTime += Time.deltaTime;
                }
            }
        }

        [SerializeField] private float amount;
        [SerializeField] private float duration;
        [SerializeField] private float cooldown;
        [SerializeField] private ShieldModifierDefinition shieldModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, cooldown, duration, amount);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
