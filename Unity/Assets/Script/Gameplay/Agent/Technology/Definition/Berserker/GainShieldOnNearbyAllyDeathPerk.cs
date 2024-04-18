using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldOnNearbyAllyDeathPerk", menuName = "Definition/Technology/Berserker/GainShieldOnNearbyAllyDeathPerk")]
    public class GainShieldOnNearbyAllyDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private ShieldModifierDefinition shieldModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, ShieldModifierDefinition shieldModifierDefinition) : base(modifiable, modifierDefinition)
            {
                this.shieldModifierDefinition = shieldModifierDefinition;
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable as ITargeteable).Faction
                    && (IModifiable)evt.AgentObject != modifiable)
                {
                    modifiable.AddModifier(shieldModifierDefinition.CreateShield(modifiable, Definition.GetValueOrThrow<float>(this, StatisticDefinition.Shield), Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration)));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private ShieldModifierDefinition shieldModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, shieldModifierDefinition);
        }
    }
}