using Game.Statistics;

namespace Game.Projectile
{
    public abstract class ProjectileParameter
    {
        public virtual string Name { get; set; }

        protected ProjectileParameter(string name)
        {
            Name = name;
        }
    }

    public abstract class ProjectileParameter<T> : ProjectileParameter
    {
        protected ProjectileParameter(string name) : base(name)
        {
        }

        public abstract T GetValue(Context context);
    }
}
