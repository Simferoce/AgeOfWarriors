namespace Game
{
    public interface IShieldable : ITargeteable
    {
        delegate void ShieldBroken(Shield shield);

        public event ShieldBroken OnShieldBroken;
        public void AddShield(Shield shield);
    }
}
