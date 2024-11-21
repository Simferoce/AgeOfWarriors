using AgeOfWarriors.Core;

namespace AgeOfWarriors.Visual
{
    public class CharacterVisual : EntityVisual
    {
        private Character character;
        private CharacterAnimator characterAnimator;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            this.character = entity as Character;
            this.characterAnimator = GetComponentInChildren<CharacterAnimator>();
        }

        public override void Refresh()
        {
            base.Refresh();
            characterAnimator.SetFloat("SpeedRatio", character.CurrentSpeed / character.Speed);
        }
    }
}