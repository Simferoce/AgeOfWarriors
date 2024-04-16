namespace Game
{
    public interface IAttackableOwner : IComponent
    {
        public bool IsActive { get; }
        public bool IsDead { get; }
        public bool IsInvulnerable { get; }
        public float Defense { get; }
        public float Health { get; set; }

        public void Death();
    }
}
