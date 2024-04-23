using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffectTransformOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private string origin;

        public override Vector3 GetPosition(Ability ability)
        {
            foreach (TransformTag tag in ability.Character.TransformTags)
            {
                if (tag.Id == origin)
                    return tag.transform.position;
            }

            return Vector3.zero;
        }
    }
}
