using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect, IDamageSource
    {
        public override void Apply()
        {
            IAttackable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(new DamageSource() { Sources = new List<IDamageSource>() { character, this } }, character.AttackPower);
        }
    }
}
