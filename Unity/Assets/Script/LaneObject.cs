using System;
using UnityEngine;

namespace Game
{
    public abstract class LaneObject : MonoBehaviour
    {
        [SerializeField] private float collisionRange;

        private float position;

        public event Action<LaneObject> Destroyed;

        public float Min => position - collisionRange;
        public float Max => position + collisionRange;
        public float Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                this.transform.position = Lane.Ground.GetPosition(position);
            }
        }
        public float CollisionRange { get => collisionRange; set => collisionRange = value; }
        public Lane Lane { get; protected set; }
        public int Direction { get; protected set; }
        public Agent Agent { get; private set; }
        public int SpawnNumber { get; private set; }

        public virtual void Spawn(Lane lane, Agent agent, int spawnNumber, float position, int direction)
        {
            lane.Add(this);
            this.Lane = lane;
            this.Position = position;
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

            if (direction < 0)
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }

        public virtual void Tick() { }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(this.transform.position + Vector3.right * collisionRange, 0.02f);
            Gizmos.DrawSphere(this.transform.position - Vector3.right * collisionRange, 0.02f);
            Gizmos.DrawLine(this.transform.position + Vector3.right * collisionRange, this.transform.position - Vector3.right * collisionRange);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}