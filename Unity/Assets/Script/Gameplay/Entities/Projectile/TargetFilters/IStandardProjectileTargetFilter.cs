using Game.Components;

namespace Game.Projectile
{
    public interface IStandardProjectileTargetFilter
    {
        public void Initialize(ProjectileEntity projectile);
        public bool Execute(Target target);
    }
}
