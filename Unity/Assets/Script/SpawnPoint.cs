using UnityEngine;

namespace Game
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Faction faction;
        [SerializeField] private int direction = 1;

        private float position;

        public Faction Faction { get => faction; }
        public Lane Lane { get; private set; }
        public float Position { get => position; }
        public int Direction { get => direction; }

        public void Initialize(Lane lane)
        {
            this.Lane = lane;
            lane.Ground.Project(this.transform.position, out position);
        }
    }
}
