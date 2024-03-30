using System;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect
    {
        public override void Apply(Character character)
        {
            ITargeteable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(character.AttackPower);
        }
    }
}
