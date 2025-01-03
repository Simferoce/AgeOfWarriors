using Game.Agent;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class BaseProjectileAbilityEffectOrigin : ProjectileAbilityEffectOrigin
    {
        public override Vector3 GetPosition(AbilityEntity ability)
        {
            return (ability.Caster.Entity as AgentEntity).Base.transform.position;
        }
    }
}
