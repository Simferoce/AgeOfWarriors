namespace Game
{
    public partial class Level
    {
        public abstract class State
        {
            protected Level level;

            protected State(Level level)
            {
                this.level = level;
            }

            public abstract void Enter();
            public abstract void Update();
            public abstract void Exit();
        }
    }
}
