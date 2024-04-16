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

            public override float? AttackPower => modifiable.TryGetCachedComponent<Character>(out Character character) && character.Health / character.MaxHealth < threshold ? amount : null;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float threshold, float amount) : base(modifiable, modifierDefinition)
            {
                this.threshold = threshold;
                this.amount = amount;
            }
        }

        [SerializeField, Range(0, 1)] private float threshold;
        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, threshold, amount);
        }
    }
}
