using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainSpeedKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainSpeedKillingBlowPerk")]
    public class GainSpeedKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private SpeedModifierDefinition speedModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, SpeedModifierDefinition speedModifierDefinition) : base(modifiable, modifierDefinition)
            {
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded += AgentObject_OnAttackLanded;
                this.speedModifierDefinition = speedModifierDefinition;
            }

            private void AgentObject_OnAttackLanded(Attack attack, float damageDealt, bool killingBlow)
            {
                if (killingBlow)
                    modifiable.AddModifier(new SpeedModifierDefinition.SpeedBuff(modifiable, speedModifierDefinition, Definition.GetValueOrThrow<float>(this, StatisticDefinition.Speed)).With(new CharacterModifierTimeElement(Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration))));
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField] private SpeedModifierDefinition speedModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, speedModifierDefinition);
        }
    }
}
