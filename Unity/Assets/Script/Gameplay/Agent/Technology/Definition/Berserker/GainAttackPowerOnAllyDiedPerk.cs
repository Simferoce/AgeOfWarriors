using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainAttackPowerOnAllyDiedPerk", menuName = "Definition/Technology/Berserker/GainAttackPowerOnAllyDiedPerk")]
    public class GainAttackPowerOnAllyDiedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private AttackPowerModifierDefinition attackPowerModifier;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, AttackPowerModifierDefinition attackPowerModifier) : base(modifiable, modifierDefinition)
            {
                this.attackPowerModifier = attackPowerModifier;

                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable as ITargeteable).Faction && (IModifiable)evt.AgentObject != modifiable)
                {
                    modifiable.AddModifier(new AttackPowerModifierDefinition.AttackPowerBuff(modifiable, attackPowerModifier, Definition.GetValueOrThrow<float>(this, StatisticDefinition.AttackPower)).With(new CharacterModifierTimeElement(Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration))));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private AttackPowerModifierDefinition attackPowerModifier;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, attackPowerModifier);
        }
    }
}
