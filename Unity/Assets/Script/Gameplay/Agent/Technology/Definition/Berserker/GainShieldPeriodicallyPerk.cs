using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldPeriodicallyPerk", menuName = "Definition/Technology/Berserker/GainShieldPeriodicallyPerk")]
    public class GainShieldPeriodicallyPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float accumulatedTime = 0.0f;
            private ShieldModifierDefinition shieldModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, ShieldModifierDefinition shieldModifierDefinition) : base(modifiable, modifierDefinition)
            {
                this.shieldModifierDefinition = shieldModifierDefinition;
            }

            public override void Update()
            {
                base.Update();

                if (modifiable.TryGetCachedComponent<Character>(out Character character) && character.IsActive && character.IsEngaged)
                {
                    if ((int)((accumulatedTime + Time.deltaTime) / Definition.GetValueOrThrow<float>(this, StatisticDefinition.Cooldown)) != (int)(accumulatedTime / Definition.GetValueOrThrow<float>(this, StatisticDefinition.Cooldown)))
                    {
                        modifiable.AddModifier(shieldModifierDefinition.CreateShield(modifiable, Definition.GetValueOrThrow<float>(this, StatisticDefinition.Shield), Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration)));
                    }

                    accumulatedTime += Time.deltaTime;
                }
            }
        }

        [SerializeField] private ShieldModifierDefinition shieldModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, shieldModifierDefinition);
        }
    }
}
