using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ProjectileAbilityEffectOrigin
    {
        public abstract Vector3 GetPosition(Ability ability);
    }
}
