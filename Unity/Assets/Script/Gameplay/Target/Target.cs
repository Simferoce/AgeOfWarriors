using UnityEngine;

namespace Game
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private Transform targetPosition;
        [SerializeField] private Collider2D hitbox;

        public Vector3 CenterPosition { get => transform.position; }
        public Vector3 TargetPosition { get => targetPosition.transform.position; }

        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
        }
    }
}
