using System;

namespace Game
{
    public interface IAttackable : IComponent
    {
        public event Action<AttackResult, IAttackable> OnDamageTaken;

        public string Name { get; }

        public void TakeAttack(Attack attack);
    }
}
