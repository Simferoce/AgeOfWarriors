namespace Game
{
    public interface ITargetOwner
    {
        public bool IsActive { get; }
        public int Priority { get; }
        public Faction Faction { get; }
    }
}
