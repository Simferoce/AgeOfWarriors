using System;

namespace Game.Statistics
{
    [Serializable]
    public abstract class ContextStatisticDefinitionData : StatisticDefinitionData
    {
        public abstract bool IsApplicable(Context context);
    }
}
