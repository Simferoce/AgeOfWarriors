using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffectTargetCenterOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private Vector3 offset = Vector3.zero;

        public override Vector3 GetPosition(Ability ability)
        {
            Vector3 closest = ability.Targets.OrderBy(x => Mathf.Abs(x.TargetPosition.x - ability.Caster.Entity.transform.position.x)).FirstOrDefault().TargetPosition;

            return closest + new Vector3(offset.x * (ability.Caster.Entity as AgentObject).Direction, offset.y);
        }
    }
}
