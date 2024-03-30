using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class AttackSource
    {
        public List<IAttackSource> Sources { get; set; } = new List<IAttackSource>();

        public AttackSource(IAttackSource source)
        {
            Sources.Add(source);
        }

        public AttackSource(List<IAttackSource> sources)
        {
            Sources.AddRange(sources);
        }

        public AttackSource Clone()
        {
            return new AttackSource(Sources.ToList());
        }
    }
}
