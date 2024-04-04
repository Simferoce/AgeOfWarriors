using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainSpeedKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainSpeedKillingBlowPerk")]
    public class GainSpeedKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public class SpeedBuff : Modifier<SpeedBuff>
            {
                private float speed;

                public override float? SpeedPercentage => speed;

                public SpeedBuff(IModifiable modifiable, float speed) : base(modifiable)
                {
                    this.speed = speed;
                }
            }

            private float speed;
            private float duration;

            public Modifier(IModifiable modifiable, float speed, float duration) : base(modifiable)
            {
                this.speed = speed;
                this.duration = duration;

                if (modifiable is AgentObject agentObject)
                    agentObject.OnAttackLanded += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(Attack attack, float damageDealt, bool killingBlow)
            {
                if (killingBlow)
                    modifiable.AddModifier(new SpeedBuff(modifiable, speed).With(new CharacterModifierTimeElement(duration)));
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable is AgentObject agentObject)
                    agentObject.OnAttackLanded -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 1)] private float speed;
        [SerializeField] private float duration;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, speed, duration);
        }
    }
}
