﻿using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainHealthKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainHealthKillingBlowPerk")]
    public class GainHealthKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainHealthKillingBlowPerk>
        {
            public Modifier(IModifiable modifiable, GainHealthKillingBlowPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(AttackResult attackResult)
            {
                if (attackResult.KillingBlow && modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.Heal(definition.percentageHealMaxHealth * character.MaxHealth);
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 1)] private float percentageHealMaxHealth;

        public override string ParseDescription()
        {
            return string.Format(this.Description, StatisticFormatter.Percentage(percentageHealMaxHealth, StatisticDefinition.MaxHealth));
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }
    }
}
