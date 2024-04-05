using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpeedModifierDefinition", menuName = "Definition/Modifier/SpeedModifierDefinition")]
    public class SpeedModifierDefinition : ModifierDefinition
    {
        public class SpeedBuff : Modifier<SpeedBuff>
        {
            private float speed;

            public override float? SpeedPercentage => speed;

            public SpeedBuff(IModifiable modifiable, ModifierDefinition modifierDefinition, float speed) : base(modifiable, modifierDefinition)
            {
                this.speed = speed;
            }
        }

    }
}
