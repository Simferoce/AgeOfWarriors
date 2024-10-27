using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public class Target : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            All = new List<Target>();
        }
        public static List<Target> All { get; private set; }

        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Transform targetPosition;

        public Vector3 CenterPosition => transform.position;
        public Vector3 TargetPosition => targetPosition.position;
        public Entity Entity { get; set; }

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
            All.Add(this);
        }

        private void OnDestroy()
        {
            All.Remove(this);
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
        }
    }
}