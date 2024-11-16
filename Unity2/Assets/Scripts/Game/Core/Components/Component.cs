namespace AgeOfWarriors.Core
{
    public abstract class Component
    {
        protected Game game;

        protected Component(Game game)
        {
            this.game = game;
        }
    }
}
