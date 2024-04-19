using System;

namespace Game
{
    public interface IAttackable
    {
        public event Action<AttackResult, IAttackable> OnDamageTaken;

        public void TakeAttack(Attack attack);
    }
}
