using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class ModifierHandler : IModifier
    {
        public float? SpeedPercentage => modifiers.Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value);
        public float? Defense => modifiers.Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value);
        public float? MaxHealth => modifiers.Where(x => x.MaxHealth.HasValue).Sum(x => x.MaxHealth.Value);
        public float? AttackPower => modifiers.Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value);
        public bool? Invulnerable => modifiers.Where(x => x.Invulnerable.HasValue).Any(x => x.Invulnerable.Value);

        public List<Modifier> Modifiers { get => modifiers; set => modifiers = value; }
        private List<Modifier> modifiers = new List<Modifier>();

        public void Add(Modifier modifier)
        {
            modifier.Initialize();
            modifiers.Add(modifier);
        }

        public void Update()
        {
            foreach (Modifier modifier in modifiers.ToList())
            {
                modifier.Update();
            }
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
