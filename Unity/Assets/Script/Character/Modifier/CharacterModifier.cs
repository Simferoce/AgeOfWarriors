namespace Game
{
    public abstract class CharacterModifier : IChararacterModifier
    {
        protected Character character;

        protected CharacterModifier(Character character)
        {
            this.character = character;
        }

        public virtual float? SpeedPercentage => null;
        public virtual float? Defense => null;

        public virtual void Update() { }
        public virtual void Refresh() { }
    }
}
