using UnityEngine;

namespace Game
{
    public interface IBlocker
    {
        public bool IsActive { get; }
        public Vector3 Position { get; }
        public int Priority { get; }
        public Faction Faction { get; }
        public Collider2D Collider { get; }

        bool IsBlocking(AgentObject agentObject);
    }
}