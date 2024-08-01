using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseMaxHealthPerk", menuName = "Definition/Technology/Common/IncreaseMaxHealthPerk")]
    public class IncreaseMaxHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseMaxHealthPerk>
        {
            public override float? MaxHealth => definition.amount;

            public Modifier(IModifiable modifiable, IncreaseMaxHealthPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField] private float amount;

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
