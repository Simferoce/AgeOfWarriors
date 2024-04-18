using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldOnNearbyAllyDeathPerk", menuName = "Definition/Technology/Berserker/GainShieldOnNearbyAllyDeathPerk")]
    public class GainShieldOnNearbyAllyDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable as ITargeteable).Faction
                    && modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable)
                    && (IModifiable)evt.AgentObject != modifiable
                    && modifiable.TryGetCachedComponent<Character>(out Character character))
                {
                    shieldable.AddShield(new Shield(Definition.GetValueOrThrow<float>(this, StatisticDefinition.Shield), Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration)));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}