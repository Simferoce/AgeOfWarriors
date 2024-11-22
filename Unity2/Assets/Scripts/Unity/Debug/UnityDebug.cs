using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class UnityDebug : IGameDebug
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
