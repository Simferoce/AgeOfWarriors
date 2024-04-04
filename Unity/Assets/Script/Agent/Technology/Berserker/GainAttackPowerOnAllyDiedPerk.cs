using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainAttackPowerOnAllyDiedPerk", menuName = "Definition/Technology/Berserker/GainAttackPowerOnAllyDiedPerk")]
    public class GainAttackPowerOnAllyDiedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public class AttackPowerBuff : Modifier<AttackPowerBuff>
            {
                private float amount;

                public override float? AttackPower => amount;

                public AttackPowerBuff(IModifiable modifiable, float amount) : base(modifiable)
                {
                    this.amount = amount;
                }
            }

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
                if (evt.AgentObject.IsAlly(modifiable as ITargeteable) && (IModifiable)evt.AgentObject != modifiable)
                {
                    modifiable.AddModifier(new AttackPowerBuff(modifiable, amount).With(new CharacterModifierTimeElement(duration)));
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

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, amount, duration);
        }
    }
}
