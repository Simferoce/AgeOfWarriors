using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainSpeedKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainSpeedKillingBlowPerk")]
    public class GainSpeedKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainSpeedKillingBlowPerk>
        {
            private SpeedModifierDefinition speedModifierDefinition;

            public Modifier(ModifierHandler modifiable, GainSpeedKillingBlowPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                if (modifiable.Entity.TryGetCachedComponent<AttackFactory>(out AttackFactory attackFactory))
                    attackFactory.OnAttackDealt += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(AttackResult attackResult)
            {
                if (attackResult.KillingBlow)
                    modifiable.AddModifier(new SpeedModifierDefinition.Modifier(
                        modifiable,
                        speedModifierDefinition,
                        definition.speed,
                        Source)
                            .With(new CharacterModifierTimeElement(definition.duration)));
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable.Entity.TryGetCachedComponent<AttackFactory>(out AttackFactory attackFactory))
                    attackFactory.OnAttackDealt -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 5)] private float speed;
        [SerializeField] private float duration;
        [SerializeField] private SpeedModifierDefinition speedModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, speed, duration);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
