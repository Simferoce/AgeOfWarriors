namespace Game
{
    public interface IHealable : ITargeteable
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public void Heal(float amount);
    }
}
