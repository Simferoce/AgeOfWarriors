namespace Game
{
    public interface ITargeteable
    {
        public Faction Faction { get; }
        public bool Attackable();
        public void Attack(float damage);
        public int Priority { get; }
    }
}