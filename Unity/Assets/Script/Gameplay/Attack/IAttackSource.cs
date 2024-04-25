namespace Game
{
    public interface IAttackSource
    {
        public void AttackLanded(AttackResult attackResult) { }
        public bool RecentlyAttacked(IAttackable attackable) { return false; }
    }
}
