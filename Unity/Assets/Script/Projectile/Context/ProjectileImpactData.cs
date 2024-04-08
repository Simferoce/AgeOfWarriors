using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileImpactData : ProjectileData
    {
        [SerializeReference, SerializeReferenceDropdown] private TargetCriteria criteria;

        public class Context : ProjectileContext
        {
            public TargetCriteria Criteria { get; set; }
        }

        public override ProjectileContext GetContext(Character character)
        {
            return new Context() { Criteria = criteria };
        }

        public override ProjectileData Clone()
        {
            ProjectileImpactData data = new ProjectileImpactData();
            data.criteria = criteria.Clone();
            return data;
        }
    }
}