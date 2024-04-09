using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileFactoryDurationContext : ProjectileFactoryContext
    {
        public class Context : ProjectileContext
        {
            public float Duration { get; set; }
        }

        [SerializeField] private float duration;

        public override ProjectileContext GetContext(Character character)
        {
            return new Context() { Duration = duration };
        }
    }
}
