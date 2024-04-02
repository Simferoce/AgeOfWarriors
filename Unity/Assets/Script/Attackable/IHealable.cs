namespace Game
{
    public interface IHealable : ITargeteable
    {
        public void Heal(float amount);
    }
}
