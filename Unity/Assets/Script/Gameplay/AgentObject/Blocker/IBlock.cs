namespace Game
{
    public interface IBlock
    {
        public bool IsActive { get; }

        public bool IsBlocking(Character character);
    }
}
