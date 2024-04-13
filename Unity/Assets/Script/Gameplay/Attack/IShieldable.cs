namespace Game
{
    public interface IShieldable
    {
        public event System.Action<IShieldable> OnDestroyed;
        delegate void ShieldBroken(Shield shield);

        public event ShieldBroken OnShieldBroken;
        public void AddShield(Shield shield);
    }
}
