namespace Game
{
    public interface ITargeteable
    {
        public Faction Faction { get; }
        public bool Attackable();
        public void TakeAttack(float damage);
        public int Priority { get; }
    }
}