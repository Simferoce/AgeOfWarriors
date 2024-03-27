using UnityEngine;

namespace Game
{
    public interface ITargeteable
    {
        public Vector3 Position { get; }
        public Faction Faction { get; }
        public bool Attackable();
        public void TakeAttack(float damage);
        public int Priority { get; }
        public bool CanBlocks(Faction faction);
    }
}