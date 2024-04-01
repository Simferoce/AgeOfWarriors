using UnityEngine;

namespace Game
{
    public interface IDisplaceable
    {
        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 Position { get; }
        public Faction Faction { get; }
        public void Displace(Vector2 displacement);
    }
}
