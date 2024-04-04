using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnRemainingHealthPerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnRemainingHealthPerk")]
    public class IncreaseAttackPowerBaseOnRemainingHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float threshold;
            private float amount;

            public override float? AttackPower => modifiable is IHealable healable && healable.Health / healable.MaxHealth < threshold ? amount : null;

            public Modifier(IModifiable modifiable, float threshold, float amount) : base(modifiable)
            {
                this.threshold = threshold;
                this.amount = amount;
            }
        }

        [SerializeField, Range(0, 1)] private float threshold;
        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, threshold, amount);
        }
    }
}
