using System;
using UnityEngine;

namespace Game
{
    public interface IAttackable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;

        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 Position { get; }
        public Faction Faction { get; }
        public bool Attackable();
        public void TakeAttack(Attack attack);
        public Collider2D Collider { get; }
    }
}
