using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffectTransformOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private Transform origin;

        public override Vector3 GetPosition(ProjectileAbilityEffect ability)
        {
            return origin.position;
        }
    }
}
