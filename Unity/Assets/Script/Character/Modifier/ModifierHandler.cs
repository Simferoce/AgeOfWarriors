using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class ModifierHandler : IModifier
    {
        public float? SpeedPercentage => modifiers.Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value);
        public float? Defense => modifiers.Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value);
        public float? MaxHealth => modifiers.Where(x => x.MaxHealth.HasValue).Sum(x => x.MaxHealth.Value);

        public List<Modifier> Modifiers { get => modifiers; set => modifiers = value; }

        private List<Modifier> modifiers = new List<Modifier>();

        public void Add(Modifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void Remove(Modifier modifier)
        {
            modifier.Dispose();
            modifiers.Remove(modifier);
        }

        public void Dispose()
        {
            foreach (Modifier modifier in modifiers)
                modifier.Dispose();
        }
    }
}
