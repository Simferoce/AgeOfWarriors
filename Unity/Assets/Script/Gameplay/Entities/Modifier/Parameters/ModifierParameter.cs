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

    public class ModifierParameter<T> : ModifierParameter
    {
        public T value;

        public ModifierParameter(string name, T value) : base(name)
        {
            this.value = value;
        }

        public T GetValue()
        {
            return value;
        }
    }
}