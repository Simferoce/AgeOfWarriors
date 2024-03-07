using UnityEngine;

namespace Game
{
    public abstract class LaneObjectDefinition : ScriptableObject
    {
        [SerializeField] private float productionDuration;
        [SerializeField] private float cost;
        [SerializeField] private Sprite icon;

        public Sprite Icon { get => icon; }
        public float ProductionDuration { get => productionDuration; set => productionDuration = value; }
        public float Cost { get => cost; set => cost = value; }

        public abstract LaneObject Spawn(Agent agent, int spawnNumber, float position, int direction);
    }
}