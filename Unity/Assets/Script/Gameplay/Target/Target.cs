using UnityEngine;

namespace Game
{
    public class Target : Entity, ITargeteable
    {
        [SerializeField] private Transform targetPosition;
        [SerializeField] private Collider2D hitbox;

        public Vector3 CenterPosition { get => transform.position; }
        public Vector3 TargetPosition { get => targetPosition.position; }
        public bool IsActive { get => owner.IsActive; }
        public int Priority { get => owner.Priority; }
        public Faction Faction { get => owner.Faction; }

        private ITargetOwner owner;

        private void Awake()
        {
            owner = GetCachedComponent<ITargetOwner>();
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
        }
    }
}
