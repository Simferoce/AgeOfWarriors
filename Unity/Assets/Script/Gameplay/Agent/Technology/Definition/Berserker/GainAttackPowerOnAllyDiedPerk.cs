using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainAttackPowerOnAllyDiedPerk", menuName = "Definition/Technology/Berserker/GainAttackPowerOnAllyDiedPerk")]
    public class GainAttackPowerOnAllyDiedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainAttackPowerOnAllyDiedPerk>
        {
            public Modifier(IModifiable modifiable, GainAttackPowerOnAllyDiedPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == modifiable.GetCachedComponent<ITargeteable>().Faction && (IModifiable)evt.AgentObject != modifiable)
                {
                    modifiable.AddModifier(
                        new AttackPowerModifierDefinition.AttackPowerBuff(modifiable,
                            definition.attackPowerModifier,
                            definition.attackPowerGain)
                        .With(new CharacterModifierTimeElement(definition.buffDuration)));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private float attackPowerGain;
        [SerializeField] private float buffDuration;
        [SerializeField] private AttackPowerModifierDefinition attackPowerModifier;

        public override string ParseDescription()
        {
            return $"Gain {attackPowerGain} Attack Power for {buffDuration} seconds whenever an ally dies.";
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
