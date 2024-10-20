namespace Game.Projectile
{
    public interface IProjectileStandardEffect
    {
        public void Initialize(ProjectileEntity projectile);
        public void Execute();
    }
}
