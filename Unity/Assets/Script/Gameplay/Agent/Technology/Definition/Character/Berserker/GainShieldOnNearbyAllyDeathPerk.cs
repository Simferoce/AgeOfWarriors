using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldOnNearbyAllyDeathPerk", menuName = "Definition/Technology/Berserker/GainShieldOnNearbyAllyDeathPerk")]
    public class GainShieldOnNearbyAllyDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainShieldOnNearbyAllyDeathPerk>
        {
            public Modifier(ModifierHandler modifiable, GainShieldOnNearbyAllyDeathPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable.Entity as AgentObject).Faction
                    && evt.AgentObject != modifiable.Entity)
                {

                    modifiable.AddModifier(definition.shieldModifierDefinition.CreateShield(modifiable, definition.amount, definition.duration));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private float duration;
        [SerializeField] private float amount;
        [SerializeField] private ShieldModifierDefinition shieldModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, amount, duration);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}