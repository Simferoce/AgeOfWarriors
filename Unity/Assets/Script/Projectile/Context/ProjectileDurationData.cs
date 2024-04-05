using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileDurationData : ProjectileData
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

        public override ProjectileData Clone()
        {
            ProjectileDurationData data = new ProjectileDurationData();
            data.duration = duration;

            return data;
        }
    }
}
