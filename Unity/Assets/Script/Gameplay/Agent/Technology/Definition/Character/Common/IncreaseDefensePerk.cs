using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefensePerk", menuName = "Definition/Technology/Common/IncreaseDefensePerk")]
    public class IncreaseDefensePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefensePerk>
        {
            public override float? Defense => definition.amount;

            public Modifier(ModifierHandler modifiable, IncreaseDefensePerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
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
