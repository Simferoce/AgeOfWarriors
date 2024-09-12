using System.Collections.Generic;

namespace Test
{
    public partial class StatisticTest : StatisticTestParent
    {
        [Statistic(AppendStatisticClassName = true)] public List<IStatisticProvider> Extension2 => new List<IStatisticProvider>();

        public void Test()
        {

        }
    }
}
