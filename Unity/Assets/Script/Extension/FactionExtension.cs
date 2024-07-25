using Game;

namespace Extension
{
    public static class FactionExtension
    {
        public static Faction GetConfusedFaction(this Faction faction)
            => faction switch
            {
                Faction.Player => Faction.Opponent,
                Faction.Opponent => Faction.Player,
                _ => faction
            };
    }
}
