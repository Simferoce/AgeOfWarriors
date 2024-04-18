using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldPeriodicallyPerk", menuName = "Definition/Technology/Berserker/GainShieldPeriodicallyPerk")]
    public class GainShieldPeriodicallyPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float accumulatedTime = 0.0f;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {

            }

            public override void Update()
            {
                base.Update();

                if (modifiable.TryGetCachedComponent<Character>(out Character character) && character.IsActive && character.IsEngaged
                    && modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    if ((int)((accumulatedTime + Time.deltaTime) / Definition.GetValueOrThrow<float>(this, StatisticDefinition.Cooldown)) != (int)(accumulatedTime / Definition.GetValueOrThrow<float>(this, StatisticDefinition.Cooldown)))
                    {
                        shieldable.AddShield(new Shield(Definition.GetValueOrThrow<float>(this, StatisticDefinition.Shield), Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration)));
                    }

                    accumulatedTime += Time.deltaTime;
                }
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
