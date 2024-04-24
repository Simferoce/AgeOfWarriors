namespace Game
{
    public interface IProjectileModifier
    {
        public bool HasModifier => true;
        public Game.Modifier GetModifier(Projectile projectile);
    }
}
