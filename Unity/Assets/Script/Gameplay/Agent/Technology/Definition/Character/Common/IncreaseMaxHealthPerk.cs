using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseMaxHealthPerk", menuName = "Definition/Technology/Common/IncreaseMaxHealthPerk")]
    public class IncreaseMaxHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseMaxHealthPerk>
        {
            public override float? MaxHealth => definition.amount;

            public Modifier(ModifierHandler modifiable, IncreaseMaxHealthPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
