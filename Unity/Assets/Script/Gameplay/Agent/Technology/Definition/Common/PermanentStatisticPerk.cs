using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PermanentStatisticPerk", menuName = "Definition/Technology/Common/PermanentStatisticPerk")]
    public class PermanentStatisticPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
