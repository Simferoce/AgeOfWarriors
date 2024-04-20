using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainSpeedKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainSpeedKillingBlowPerk")]
    public class GainSpeedKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainSpeedKillingBlowPerk>
        {
            private SpeedModifierDefinition speedModifierDefinition;

            public Modifier(IModifiable modifiable, GainSpeedKillingBlowPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(AttackResult attackResult)
            {
                if (attackResult.KillingBlow)
                    modifiable.AddModifier(new SpeedModifierDefinition.SpeedBuff(modifiable, speedModifierDefinition, definition.speed)
                        .With(new CharacterModifierTimeElement(definition.duration)));
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 5)] private float speed;
        [SerializeField] private float duration;
        [SerializeField] private SpeedModifierDefinition speedModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
