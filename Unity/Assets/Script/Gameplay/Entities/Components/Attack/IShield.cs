namespace Game.Components
{
    public interface IShield
    {
        public float InitialAmount { get; }
        public float Remaining { get; }
        public float Absorb(float damage);
    }
}
