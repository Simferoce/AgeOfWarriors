namespace Game
{
    public static class StatisticFormatter
    {
        public static string Percentage(float value, StatisticDefinition definition)
        {
            return $"<color=#{definition.ColorHex}>({value:0.0%}{definition.TextIcon})</color>";
        }
    }
}
