using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldOnNearbyAllyDeathPerk", menuName = "Definition/Technology/Berserker/GainShieldOnNearbyAllyDeathPerk")]
    public class GainShieldOnNearbyAllyDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainShieldOnNearbyAllyDeathPerk>
        {
            public Modifier(IModifiable modifiable, GainShieldOnNearbyAllyDeathPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == modifiable.GetCachedComponent<ITargeteable>().Faction
                    && (IModifiable)evt.AgentObject != modifiable)
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

        [Statistic("shield")] public float Amount(Modifier modifier) => amount;
        [Statistic("buff_duration")] public float Duration(Modifier modifier) => duration;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}