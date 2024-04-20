using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpeedModifierDefinition", menuName = "Definition/Modifier/SpeedModifierDefinition")]
    public class SpeedModifierDefinition : ModifierDefinition
    {
        public class SpeedBuff : Modifier<SpeedBuff, SpeedModifierDefinition>
        {
            private float speed;

            public override float? SpeedPercentage => speed;

            public SpeedBuff(IModifiable modifiable, SpeedModifierDefinition modifierDefinition, float speed) : base(modifiable, modifierDefinition)
            {
                this.speed = speed;
            }
        }

    }
}
