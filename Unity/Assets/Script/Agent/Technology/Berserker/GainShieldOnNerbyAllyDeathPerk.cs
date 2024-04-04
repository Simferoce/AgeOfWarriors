using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainShieldOnNerbyAllyDeathPerk", menuName = "Definition/Technology/Berserker/GainShieldOnNerbyAllyDeathPerk")]
    public class GainShieldOnNerbyAllyDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float amount;
            private float duration;

            public Modifier(IModifiable modifiable, float amount, float duration) : base(modifiable)
            {
                this.amount = amount;
                this.duration = duration;

                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.IsAlly(modifiable as ITargeteable) && modifiable is Character shieldable && (IModifiable)evt.AgentObject != modifiable)
                {
                    shieldable.AddShield(new Shield(amount * shieldable.MaxHealth, duration));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField, Range(0, 1)] private float amount;
        [SerializeField] private float duration;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, amount, duration);
        }
    }
}
