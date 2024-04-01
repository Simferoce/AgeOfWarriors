using UnityEngine;

namespace Game
{
    public interface ITargeteable
    {
        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 CenterPosition { get; }
        public Vector3 TargetPosition { get; }
        public Faction Faction { get; }

        public bool IsAttackable();
        public bool IsDisplaceable();
        public bool IsAlly(ITargeteable targeteable);
        public bool IsEnemy(ITargeteable targeteable);
    }
}
