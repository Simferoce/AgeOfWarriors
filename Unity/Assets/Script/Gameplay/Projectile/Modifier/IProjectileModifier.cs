namespace Game
{
    public interface IProjectileModifier
    {
        public Game.Modifier GetModifier(Projectile projectile);
    }
}
