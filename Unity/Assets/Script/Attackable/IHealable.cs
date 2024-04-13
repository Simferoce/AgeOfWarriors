namespace Game
{
    public interface IHealable
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public void Heal(float amount);
    }
}
