using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldOnNearbyAllyDeathPerk", menuName = "Definition/Technology/Berserker/GainShieldOnNearbyAllyDeathPerk")]
    public class GainShieldOnNearbyAllyDeathPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainShieldOnNearbyAllyDeathPerk>
        {
            public Modifier(GainShieldOnNearbyAllyDeathPerk modifierDefinition) : base(modifierDefinition)
            {
                DeathEventChannel.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(DeathEventChannel.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable.Entity as AgentObject).Faction
                    && evt.AgentObject != modifiable.Entity)
                {
                    Source.Apply(modifiable, definition.shieldModifierDefinition.CreateShield(definition.amount, definition.duration));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                DeathEventChannel.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private float duration;
        [SerializeField] private float amount;
        [SerializeField] private ShieldModifierDefinition shieldModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, amount, duration);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}