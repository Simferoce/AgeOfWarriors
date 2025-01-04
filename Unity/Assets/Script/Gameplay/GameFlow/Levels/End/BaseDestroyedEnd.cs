namespace Game
{
    public partial class Level
    {
        public class BaseDestroyedEnd : End
        {
            public FactionType Faction { get; private set; }

            public BaseDestroyedEnd(FactionType faction)
            {
                Faction = faction;
            }
        }
    }
}
