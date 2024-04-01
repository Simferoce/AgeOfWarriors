using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect, IAttackSource
    {
        [SerializeField, Range(0, 1)] private float leach = 0f;
        [SerializeField, Range(0, 3)] private float damagePercentage = 1f;

        public override void Apply()
        {
            IAttackable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(new Attack(new AttackSource(new List<IAttackSource>() { character, this }), damagePercentage * character.AttackPower, leach));
        }
    }
}
