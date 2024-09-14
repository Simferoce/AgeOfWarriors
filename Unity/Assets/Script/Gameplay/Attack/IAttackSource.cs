namespace Game
{
    public interface IAttackSource
    {
        public void AttackLanded(AttackResult attackResult) { }
        public bool RecentlyAttacked(Attackable attackable) { return false; }
    }
}
