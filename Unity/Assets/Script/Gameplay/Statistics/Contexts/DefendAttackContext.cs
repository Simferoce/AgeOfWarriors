using Game.Components;

namespace Game.Statistics
{
    public class DefendAttackContext : Context
    {
        public AttackData AttackData { get; private set; }

        public DefendAttackContext(AttackData attackData)
        {
            AttackData = attackData;
        }
    }
}
