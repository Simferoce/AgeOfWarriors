using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GiveDefenseOnDeathModifier", menuName = "Definition/Technology/Shieldbearer/GiveDefenseOnDeathModifier")]
    public class GiveDefenseOnDeathModifier : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GiveDefenseOnDeathModifier>
        {
            public Modifier(GiveDefenseOnDeathModifier modifierDefinition) : base(modifierDefinition)
            {
            }
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
