using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffectTargetCenterOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private Vector3 offset = Vector3.zero;

        public override Vector3 GetPosition(ProjectileAbilityEffect ability)
        {
            Vector3 closest = ability.Ability.Targets.OrderBy(x => Mathf.Abs(x.TargetPosition.x - ability.Ability.Character.CenterPosition.x)).FirstOrDefault().TargetPosition;

            return closest + new Vector3(offset.x * ability.Ability.Character.Direction, offset.y);
        }
    }
}
