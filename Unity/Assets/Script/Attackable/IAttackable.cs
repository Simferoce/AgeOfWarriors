using System;
using UnityEngine;

namespace Game
{
    public interface IAttackable : ITargeteable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;

        public void TakeAttack(Attack attack);
        public void Stagger(float duration);
        public Vector3 ClosestPoint(Vector3 point);
    }
}
