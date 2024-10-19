using Game;

namespace Extensions
{
    public static class FactionTypeExtension
    {
        public static FactionType GetConfusedFaction(this FactionType faction)
            => faction switch
            {
                FactionType.Player => FactionType.Opponent,
                FactionType.Opponent => FactionType.Player,
                _ => faction
            };
    }
}
