using UnityEngine;

namespace Game
{
    public interface IDisplaceable : IComponent
    {
        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 CenterPosition { get; }
        public Faction Faction { get; }

        public void Displace(Vector2 displacement);
    }
}
