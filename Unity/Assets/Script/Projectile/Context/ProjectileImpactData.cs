using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileImpactData : ProjectileData
    {
        [SerializeField] private List<Tag> tags;

        public class Context : ProjectileContext
        {
            public List<Tag> Tags { get; set; }
        }

        public override ProjectileContext GetContext(Character character)
        {
            return new Context() { Tags = tags };
        }
    }
}