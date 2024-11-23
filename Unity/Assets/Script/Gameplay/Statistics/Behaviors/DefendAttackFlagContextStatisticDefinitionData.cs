using Game.Components;
using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class DefendAttackFlagContextStatisticDefinitionData : ContextStatisticDefinitionData
    {
        [SerializeField] private AttackData.Flag flag;

        public override bool IsApplicable(Context context)
        {
            if (context is not DefendAttackContext attackContext)
                return false;

            return attackContext.AttackData.Flags.HasFlag(flag);
        }
    }
}
