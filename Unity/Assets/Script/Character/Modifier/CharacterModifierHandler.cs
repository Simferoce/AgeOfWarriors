using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class CharacterModifierHandler : IChararacterModifier
    {
        public float? SpeedPercentage => modifiers.Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value);
        public float? Defense => modifiers.Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value);

        public List<CharacterModifier> Modifiers { get => modifiers; set => modifiers = value; }


        private List<CharacterModifier> modifiers = new List<CharacterModifier>();

        public void Add(CharacterModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void Remove(CharacterModifier modifier)
        {
            modifiers.Remove(modifier);
        }
    }
}
