using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainAttackPowerOnAllyDiedPerk", menuName = "Definition/Technology/Berserker/GainAttackPowerOnAllyDiedPerk")]
    public class GainAttackPowerOnAllyDiedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float amount;
            private float duration;
            private AttackPowerModifierDefinition attackPowerModifier;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float amount, float duration, AttackPowerModifierDefinition attackPowerModifier) : base(modifiable, modifierDefinition)
            {
                this.amount = amount;
                this.duration = duration;
                this.attackPowerModifier = attackPowerModifier;

                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable as ITargeteable).Faction && (IModifiable)evt.AgentObject != modifiable)
                {
                    modifiable.AddModifier(new AttackPowerModifierDefinition.AttackPowerBuff(modifiable, attackPowerModifier, amount).With(new CharacterModifierTimeElement(duration)));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private float amount;
        [SerializeField] private float duration;
        [SerializeField] private AttackPowerModifierDefinition attackPowerModifier;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, amount, duration, attackPowerModifier);
        }
    }
}
