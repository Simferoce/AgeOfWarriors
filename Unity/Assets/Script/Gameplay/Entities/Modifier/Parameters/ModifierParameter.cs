using Game.Statistics;

namespace Game.Modifier
{
    public abstract class ModifierParameter
    {
        public virtual string Name { get; set; }

        protected ModifierParameter(string name)
        {
            Name = name;
        }
    }

    public abstract class ModifierParameter<T> : ModifierParameter
    {
        protected ModifierParameter(string name) : base(name)
        {
        }

        public abstract T GetValue(Context context);
    }
}