namespace Game
{
    public class Factory
    {
        private int currentSpawnNumber;

        public void SpawnLaneObject(Agent agent, SpawnPoint spawnPoint, LaneObjectDefinition laneObjectDefinition)
        {
            laneObjectDefinition.Spawn(agent, currentSpawnNumber++, spawnPoint.Position, spawnPoint.Direction);
        }
    }
}
