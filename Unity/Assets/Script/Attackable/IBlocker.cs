using UnityEngine;

namespace Game
{
    public interface IBlocker : ITargeteable
    {
        public Collider2D Collider { get; }

        bool IsBlocking(AgentObject agentObject);
    }
}