namespace Test
{
    [StatisticClass("parent")]
    public partial class StatisticTestParent
    {
        [Statistic("attack")] public float Attack { get => 0f; }
        [Statistic("confused")] public bool Confused { get => false; }

        [Statistic("extension")] public IStatisticProvider Extension => new StatisticExtensionTest();
        [Statistic("")] public IStatisticProvider Extension2 => new StatisticExtensionTest();
        [Statistic("")] public IStatisticProvider Extension3 => new StatisticExtensionTest();

        public void Test2()
        {
        }
    }
}
