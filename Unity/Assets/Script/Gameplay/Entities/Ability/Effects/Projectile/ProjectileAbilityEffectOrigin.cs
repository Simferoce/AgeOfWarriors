using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public abstract class ProjectileAbilityEffectOrigin
    {
        public abstract Vector3 GetPosition(AbilityEntity ability);
    }
}
