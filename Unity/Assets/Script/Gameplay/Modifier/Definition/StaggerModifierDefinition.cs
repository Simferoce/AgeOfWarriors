using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerModifierDefinition", menuName = "Definition/Modifier/StaggerModifierDefinition")]
    public class StaggerModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerModifierDefinition>
        {
            public override bool? IsStagger => true;

            public Modifier(ModifierHandler modifiable, StaggerModifierDefinition modifierDefinition, float duration, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                With(new CharacterModifierTimeElement(duration));
            }

            public override void Dispose()
            {
                base.Dispose();
            }
        }
    }
}
