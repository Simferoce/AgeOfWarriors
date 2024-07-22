using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpeedModifierDefinition", menuName = "Definition/Modifier/SpeedModifierDefinition")]
    public class SpeedModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpeedModifierDefinition>
        {
            private float speed;

            public override float? SpeedPercentage => speed;

            public Modifier(IModifiable modifiable, SpeedModifierDefinition modifierDefinition, float speed, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.speed = speed;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, Mathf.Abs(speed));
            }
        }
    }
}
