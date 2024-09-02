namespace Test
{
    [StatisticClass("extension")]
    public partial class StatisticExtensionTest
    {
        [Statistic("value")] public float Value => 0.5f;
    }
}
