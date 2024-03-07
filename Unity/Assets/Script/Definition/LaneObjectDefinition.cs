using UnityEngine;

namespace Game
{
    public abstract class LaneObjectDefinition : ScriptableObject
    {
        public abstract LaneObject Spawn(Agent agent, int spawnNumber, float position, int direction);
    }
}