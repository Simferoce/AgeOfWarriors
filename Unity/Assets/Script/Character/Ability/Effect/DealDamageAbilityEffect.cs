using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect, IAttackSource
    {
        public override void Apply()
        {
            IAttackable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(new Attack(new AttackSource(new List<IAttackSource>() { character, this }), character.AttackPower, 0f));
        }
    }
}
