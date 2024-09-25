using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerModifierDefinition", menuName = "Definition/Modifier/StaggerModifierDefinition")]
    public class StaggerModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerModifierDefinition>
        {
            //public override float? IncreaseDamageTaken
            //{
            //    get
            //    {
            //        IncreaseDamageTakenWhenStaggerPerk.Modifier modifier = (Source.Entity.GetCachedComponent<ModifierHandler>().GetModifiers().FirstOrDefault(x => x is IncreaseDamageTakenWhenStaggerPerk.Modifier) as IncreaseDamageTakenWhenStaggerPerk.Modifier);
            //        return modifier?.IncreaseDamageTakenOfStaggered ?? 0f;
            //    }
            //}

            public Modifier(ModifierHandler modifiable, StaggerModifierDefinition modifierDefinition, float duration, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                With(new CharacterModifierTimeElement(duration));
            }
        }
    }
}
