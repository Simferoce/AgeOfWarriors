namespace AgeOfWarriors.Core
{
    public interface IGameDebug
    {
        public void Log(string message);
        public void LogError(string message);
    }
}
