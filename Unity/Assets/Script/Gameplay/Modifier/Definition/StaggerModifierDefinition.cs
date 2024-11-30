using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerModifierDefinition", menuName = "Definition/Modifier/StaggerModifierDefinition")]
    public class StaggerModifierDefinition : ModifierDefinition
    {
        public override Game.Modifier Instantiate()
        {
            throw new System.NotImplementedException();
        }

        public class Modifier : Modifier<Modifier, StaggerModifierDefinition>
        {
            public override bool? IsStagger => true;

            public Modifier(StaggerModifierDefinition modifierDefinition, float duration) : base(modifierDefinition)
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
