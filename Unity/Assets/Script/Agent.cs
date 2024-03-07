using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Agent : MonoBehaviour
    {
        public static List<Agent> agents = new List<Agent>();

        [SerializeField] private Faction faction;

        private int currentSpawnNumber;

        public Faction Faction { get => faction; }

        private void OnEnable()
        {
            agents.Add(this);
        }

        private void OnDisable()
        {
            agents.Remove(this);
        }

        public void SpawnLaneObject(Lane lane, LaneObjectDefinition laneObjectDefinition)
        {
            SpawnPoint spawnPoint = lane.SpawnPoints.Where(x => x.Faction == faction).FirstOrDefault();

            laneObjectDefinition.Spawn(lane, this, currentSpawnNumber++, spawnPoint.Position, spawnPoint.Direction);
        }
    }
}
