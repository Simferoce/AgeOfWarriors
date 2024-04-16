using UnityEngine;

namespace Game
{
    public interface ITargeteable : IComponent
    {
        public bool IsActive { get; }
        public int Priority { get; }
        public Faction Faction { get; }
        public Vector3 CenterPosition { get; }
        public Vector3 TargetPosition { get; }

        public Vector3 ClosestPoint(Vector3 point);
    }
}
