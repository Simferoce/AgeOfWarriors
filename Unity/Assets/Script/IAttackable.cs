using System;
using UnityEngine;

namespace Game
{
    public interface IAttackable
    {
        public event Action<DamageSource, IAttackable> OnDamageTaken;

        public int Priority { get; }
        public Vector3 Position { get; }
        public Faction Faction { get; }
        public bool Attackable();
        public void TakeAttack(DamageSource source, float damage);
    }
}
