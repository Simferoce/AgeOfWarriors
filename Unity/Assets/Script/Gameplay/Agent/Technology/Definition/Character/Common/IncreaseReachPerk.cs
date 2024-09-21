using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseReachPerk", menuName = "Definition/Technology/Common/IncreaseReachPerk")]
    public class IncreaseReachPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseReachPerk>
        {
            public override float? AttackPower => definition.amount;

            public Modifier(ModifierHandler modifiable, IncreaseReachPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
