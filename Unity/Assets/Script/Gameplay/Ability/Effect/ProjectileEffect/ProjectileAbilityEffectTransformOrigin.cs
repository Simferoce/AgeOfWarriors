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
            foreach (TransformTag tag in ability.Ability.Character.TransformTags)
            {
                if (tag.Id == origin)
                    return tag.transform.position;
            }

            return Vector3.zero;
        }
    }
}
