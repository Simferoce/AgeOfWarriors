namespace Game
{
    public interface IShieldable : IComponent
    {
        delegate void ShieldBroken(ShieldModifierDefinition.Shield shield);
        public event System.Action<IShieldable> OnShieldableDestroyed;
    }
}
