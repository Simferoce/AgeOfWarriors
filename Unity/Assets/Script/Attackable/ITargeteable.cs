using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ITargeteable
    {
        delegate void DeathHandler(ITargeteable targeteable);
        public event DeathHandler OnDeath;

        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 CenterPosition { get; }
        public Vector3 TargetPosition { get; }
        public Faction Faction { get; }

        public Vector3 ClosestPoint(Vector3 point);

        public bool IsEngaged();
        public bool IsAttackable();
        public bool IsDisplaceable();
        public bool IsInjured();
        public bool IsAlly(ITargeteable targeteable);
        public bool IsEnemy(ITargeteable targeteable);
        public List<AgentObject.Type> Types { get; }
    }
}
