using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldPeriodicallyPerk", menuName = "Definition/Technology/Berserker/GainShieldPeriodicallyPerk")]
    public class GainShieldPeriodicallyPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float amount;
            private float duration;
            private float period;

            private float accumulatedTime = 0.0f;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float amount, float duration, float period) : base(modifiable, modifierDefinition)
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
                    if ((int)((accumulatedTime + Time.deltaTime) / period) != (int)(accumulatedTime / period))
                    {
                        shieldable.AddShield(new Shield(amount * character.MaxHealth, duration));
                    }

                    accumulatedTime += Time.deltaTime;
                }
            }
        }

        [SerializeField, Range(0, 1)] private float amount;
        [SerializeField] private float duration;
        [SerializeField] private float period;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, amount, duration, period);
        }
    }
}
