using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainSpeedKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainSpeedKillingBlowPerk")]
    public class GainSpeedKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float speed;
            private float duration;
            private SpeedModifierDefinition speedModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float speed, float duration, SpeedModifierDefinition speedModifierDefinition) : base(modifiable, modifierDefinition)
            {
                this.speed = speed;
                this.duration = duration;

                if (modifiable is AgentObject agentObject)
                    agentObject.OnAttackLanded += AgentObject_OnAttackLanded;
                this.speedModifierDefinition = speedModifierDefinition;
            }

            private void AgentObject_OnAttackLanded(Attack attack, float damageDealt, bool killingBlow)
            {
                if (killingBlow)
                    modifiable.AddModifier(new SpeedModifierDefinition.SpeedBuff(modifiable, speedModifierDefinition, speed).With(new CharacterModifierTimeElement(duration)));
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
        [SerializeField] private SpeedModifierDefinition speedModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, speed, duration, speedModifierDefinition);
        }
    }
}
