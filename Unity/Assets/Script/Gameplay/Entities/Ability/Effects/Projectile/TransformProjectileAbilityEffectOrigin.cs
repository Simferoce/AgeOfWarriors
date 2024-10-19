using Game.Utilities;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class TransformProjectileAbilityEffectOrigin : ProjectileAbilityEffectOrigin
    {
        [SerializeField] private string origin;

        public override Vector3 GetPosition(AbilityEntity ability)
        {
            foreach (TransformTag tag in ability.Caster.TransformTags)
            {
                if (tag.Id == origin)
                    return tag.transform.position;
            }

            return Vector3.zero;
        }
    }
}
