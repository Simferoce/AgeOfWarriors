using System;

namespace Game
{
    public interface IAttackable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;

        public void TakeAttack(Attack attack);
    }
}
