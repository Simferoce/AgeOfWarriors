using Game.Components;

namespace Game.Statistics
{
    public class AttackContext : Context
    {
        public Attackable Target { get; set; }

        public AttackContext(Attackable target)
        {
            Target = target;
        }
    }
}
