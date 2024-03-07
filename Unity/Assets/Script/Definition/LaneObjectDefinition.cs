using UnityEngine;

namespace Game
{
    public abstract class LaneObjectDefinition : ScriptableObject
    {
        public abstract LaneObject Spawn(Lane lane, Agent agent, int spawnNumber, float position, int direction);
    }
}