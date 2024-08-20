using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ICaster : IComponent
    {
        public float LastAbilityUsed { get; set; }
        public Faction Faction { get; }
        public Vector3 CenterPosition { get; }
        public Agent Agent { get; }
        public int Direction { get; }
        public List<TransformTag> TransformTags { get; }

        public void BeginCast();
        public void EndCast();
        public List<ITargeteable> GetTargets(TargetCriteria criteria, IContext context);
    }
}
