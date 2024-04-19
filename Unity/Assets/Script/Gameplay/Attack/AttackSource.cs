using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class AttackSource
    {
        public List<IAttackSource> Sources { get; set; } = new List<IAttackSource>();

        public AttackSource(params IAttackSource[] source)
        {
            Sources.AddRange(source);
        }

        public AttackSource(List<IAttackSource> sources)
        {
            Sources.AddRange(sources);
        }

        public AttackSource Add(params IAttackSource[] source)
        {
            Sources.AddRange(source);
            return this;
        }

        public AttackSource Clone()
        {
            return new AttackSource(Sources.ToList());
        }
    }
}
