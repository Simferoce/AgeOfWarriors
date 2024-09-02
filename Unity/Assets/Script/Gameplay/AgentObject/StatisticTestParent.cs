namespace Test
{
    [StatisticClass]
    public partial class StatisticTestParent
    {
        [Statistic("attack")] public float Attack { get => 0f; }
        [Statistic("confused")] public bool Confused { get => false; }

        public void Test2()
        {
        }
    }
}
