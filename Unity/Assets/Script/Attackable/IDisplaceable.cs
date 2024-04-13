using UnityEngine;

namespace Game
{
    public interface IDisplaceable
    {
        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 CenterPosition { get; }
        public Faction Faction { get; }
        public bool IsDisplaceable();

        public void Displace(Vector2 displacement);
    }
}
