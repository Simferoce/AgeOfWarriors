using UnityEngine;

namespace Game
{
    public abstract class AgentObject : MonoBehaviour
    {
        public int Direction { get; protected set; }
        public Agent Agent { get; private set; }
        public int SpawnNumber { get; private set; }

        public virtual void Spawn(Agent agent, int spawnNumber, int direction)
        {
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

            if (direction < 0)
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
    }
}