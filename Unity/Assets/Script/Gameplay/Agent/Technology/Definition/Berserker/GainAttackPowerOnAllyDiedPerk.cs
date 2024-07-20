using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainAttackPowerOnAllyDiedPerk", menuName = "Definition/Technology/Berserker/GainAttackPowerOnAllyDiedPerk")]
    public class GainAttackPowerOnAllyDiedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainAttackPowerOnAllyDiedPerk>
        {
            public Modifier(IModifiable modifiable, GainAttackPowerOnAllyDiedPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == modifiable.GetCachedComponent<ITargeteable>().Faction && (IModifiable)evt.AgentObject != modifiable)
                {
                    modifiable.AddModifier(
                        new AttackPowerModifierDefinition.AttackPowerModifier(modifiable,
                            definition.attackPowerModifier,
                            definition.attackPowerGain,
                            Source)
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
            return string.Format(Description, attackPowerGain, buffDuration);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }
    }
}
