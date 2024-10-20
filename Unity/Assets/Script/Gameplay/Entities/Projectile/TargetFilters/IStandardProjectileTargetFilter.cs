using Game.Components;

namespace Game.Projectile
{
    public interface IStandardProjectileTargetFilter
    {
        public bool Execute(Target target);
    }
}
