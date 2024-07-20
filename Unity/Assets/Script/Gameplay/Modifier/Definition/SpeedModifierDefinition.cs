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

            public SpeedBuff(IModifiable modifiable, SpeedModifierDefinition modifierDefinition, float speed, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.speed = speed;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, speed);
            }
        }
    }
}
