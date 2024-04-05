using System;

namespace Game
{
    [Serializable]
    public class IsEnemyTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable)
        {
            return targeteable.IsEnemy(owner);
        }

        public override TargetCriteria Clone()
        {
            return new IsEnemyTargetCriteria();
        }
    }
}
