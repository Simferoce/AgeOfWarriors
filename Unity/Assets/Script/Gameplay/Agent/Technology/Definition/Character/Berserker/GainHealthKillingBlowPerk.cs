using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainHealthKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainHealthKillingBlowPerk")]
    public class GainHealthKillingBlowPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainHealthKillingBlowPerk>
        {
            public Modifier(GainHealthKillingBlowPerk modifierDefinition) : base(modifierDefinition)
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
                if (attackResult.KillingBlow && modifiable.Entity.TryGetCachedComponent<Character>(out Character character))
                    character.Heal(definition.percentageHealMaxHealth * character.MaxHealth);
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable.Entity.TryGetCachedComponent<AttackFactory>(out AttackFactory attackFactory))
                    attackFactory.OnAttackDealt -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 1)] private float percentageHealMaxHealth;

        public override string ParseDescription()
        {
            return string.Format(this.Description, StatisticFormatter.Percentage(percentageHealMaxHealth, StatisticDefinition.MaxHealth));
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
