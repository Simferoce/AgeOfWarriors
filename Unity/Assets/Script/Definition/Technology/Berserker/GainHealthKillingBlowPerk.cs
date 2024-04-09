using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainHealthKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainHealthKillingBlowPerk")]
    public class GainHealthKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float amount;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float amount) : base(modifiable, modifierDefinition)
            {
                this.amount = amount;
                if (modifiable is AgentObject agentObject)
                    agentObject.OnAttackLanded += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(Attack attack, float damageDealt, bool killingBlow)
            {
                if (killingBlow && modifiable is IHealable healable)
                    healable.Heal(amount * healable.MaxHealth);
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable is AgentObject agentObject)
                    agentObject.OnAttackLanded -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, amount);
        }
    }
}
