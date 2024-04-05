using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffectTransformOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private string origin;

        public override Vector3 GetPosition(ProjectileAbilityEffect ability)
        {
            foreach (Transform transform in ability.Ability.Character.transform)
            {
                if (transform.name == origin)
                    return transform.position;
            }

            return Vector3.zero;
        }

        public override ProjectileAbilityEffectOrigin Clone()
        {
            ProjectileAbilityEffectTransformOrigin projectileAbilityEffectTransformOrigin = new ProjectileAbilityEffectTransformOrigin();
            projectileAbilityEffectTransformOrigin.origin = origin;
            return projectileAbilityEffectTransformOrigin;
        }
    }
}
