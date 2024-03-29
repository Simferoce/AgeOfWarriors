using System;

namespace Game
{
    [Serializable]
    public class CharacterAbilityMelee : CharacterAbility
    {
        protected override void OnAnimatorEventAbilityUsed()
        {
            ITargeteable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(character.AttackPower);
        }
    }
}
