using Game.Agent;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class TargetCenterProjectileAbilityEffectOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private Vector3 offset = Vector3.zero;

        public override Vector3 GetPosition(AbilityEntity ability)
        {
            Vector3 closest = ability.Targets.OrderBy(x => Mathf.Abs(x.TargetPosition.x - ability.Caster.Entity.transform.position.x)).FirstOrDefault().TargetPosition;

            return closest + new Vector3(offset.x * ability.Caster.Entity.GetCachedComponent<AgentIdentity>().Direction, offset.y);
        }
    }
}
