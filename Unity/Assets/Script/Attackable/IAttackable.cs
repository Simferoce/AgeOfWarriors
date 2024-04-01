using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IAttackable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;

        public bool IsActive { get; }
        public int Priority { get; }
        public Vector3 TargetPosition { get; }
        public Faction Faction { get; }
        public bool IsAttackable();
        public void TakeAttack(Attack attack);
        public void Stagger(float duration);
        public Vector3 ClosestPoint(Vector3 point);

        public List<Tag> EvaluateContextualTags(IAttackable attackable);
    }
}
