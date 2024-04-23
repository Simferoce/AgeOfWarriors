using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "RangedReductionDamagePerk", menuName = "Definition/Technology/Shieldbearer/RangedReductionDamagePerk")]
    public class RangedReductionDamagePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, RangedReductionDamagePerk>
        {
            public override float? RangedDamageReduction => definition.rangedDamageReduction;

            public Modifier(IModifiable modifiable, RangedReductionDamagePerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        [SerializeField, Range(0, 1)] private float rangedDamageReduction;

        public override string ParseDescription()
        {
            return string.Format(Description, rangedDamageReduction);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
