using UnityEngine;

namespace Game
{
    public interface ITargeteable
    {
        public Vector3 Position { get; }
        public int Priority { get; }
        public Faction Faction { get; }
        public bool CanBlocks(Faction faction);
    }
}