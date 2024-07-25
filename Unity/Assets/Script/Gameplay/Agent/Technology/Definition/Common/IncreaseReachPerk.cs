using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseReachPerk", menuName = "Definition/Technology/Common/IncreaseReachPerk")]
    public class IncreaseReachPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseReachPerk>
        {
            public override float? AttackPower => definition.amount;

            public Modifier(IModifiable modifiable, IncreaseReachPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount);
        }
    }
}
