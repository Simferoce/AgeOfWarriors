using System;

namespace Game
{
    public interface IAttackable : ITargeteable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;

        public void TakeAttack(Attack attack);
        public void Stagger(float duration);
    }
}
