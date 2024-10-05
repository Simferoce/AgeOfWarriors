using Game;
using UnityEngine;

public class Target : MonoBehaviour, IComponent
{
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private Transform targetPosition;

    public Vector3 CenterPosition => transform.position;
    public Vector3 TargetPosition => targetPosition.position;

    public Entity Entity { get; set; }

    private void Awake()
    {
        Entity = GetComponentInParent<Entity>();
    }

    public Vector3 ClosestPoint(Vector3 point)
    {
        return hitbox.ClosestPoint(point);
    }
}