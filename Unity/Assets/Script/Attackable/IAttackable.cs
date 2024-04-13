using System;

namespace Game
{
    public interface IAttackable
    {
        delegate void DeathHandler(IAttackable attackable);
        public event Action<Attack, IAttackable> OnDamageTaken;

        public void TakeAttack(Attack attack);
        public void Stagger(float duration);
    }
}
