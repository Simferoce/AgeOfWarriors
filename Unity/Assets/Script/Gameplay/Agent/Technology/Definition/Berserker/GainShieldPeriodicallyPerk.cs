using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldPeriodicallyPerk", menuName = "Definition/Technology/Berserker/GainShieldPeriodicallyPerk")]
    public class GainShieldPeriodicallyPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private StatisticReference<float> amount;
            private StatisticReference<float> duration;
            private StatisticReference<float> period;

            private float accumulatedTime = 0.0f;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, StatisticReference<float> amount, StatisticReference<float> duration, StatisticReference<float> period) : base(modifiable, modifierDefinition)
            {
                this.amount = amount;
                this.duration = duration;
                this.period = period;
            }

            public override void Update()
            {
                base.Update();

                if (modifiable.TryGetCachedComponent<Character>(out Character character) && character.IsActive && character.IsEngaged
                    && modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    if ((int)((accumulatedTime + Time.deltaTime) / period.GetValueOrThrow(this)) != (int)(accumulatedTime / period.GetValueOrThrow(this)))
                    {
                        shieldable.AddShield(new Shield(amount.GetValueOrThrow(this), duration.GetValueOrThrow(this)));
                    }

                    accumulatedTime += Time.deltaTime;
                }
            }
        }

        [SerializeField] private StatisticReference<float> amount;
        [SerializeField] private StatisticReference<float> duration;
        [SerializeField] private StatisticReference<float> period;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, amount, duration, period);
        }
    }
}
