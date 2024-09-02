namespace Test
{
    [StatisticClass("parent")]
    public partial class StatisticTestParent
    {
        [Statistic("attack")] public float Attack { get => 0f; }
        [Statistic("confused")] public bool Confused { get => false; }

        [Statistic("")] public IStatisticProvider Extension => new StatisticExtensionTest();

        public void Test2()
        {
        }
    }
}
