using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainSpeedKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainSpeedKillingBlowPerk")]
    public class GainSpeedKillingBlowPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainSpeedKillingBlowPerk>
        {
            private SpeedModifierDefinition speedModifierDefinition;

            public Modifier(GainSpeedKillingBlowPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                if (modifiable.Entity.TryGetCachedComponent<AttackFactory>(out AttackFactory attackFactory))
                    attackFactory.OnAttackDealt += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(AttackResult attackResult)
            {
                if (attackResult.KillingBlow)
                {
                    Source.Apply(modifiable, new SpeedModifierDefinition.Modifier(
                        speedModifierDefinition,
                        definition.speed).With(new CharacterModifierTimeElement(definition.duration)));
                }
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

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
