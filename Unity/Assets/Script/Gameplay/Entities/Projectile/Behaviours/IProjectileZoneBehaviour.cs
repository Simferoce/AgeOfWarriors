using UnityEngine;

namespace Game.Projectile
{
    public interface IProjectileZoneBehaviour
    {
        public void EnterZone(Collider2D collider);
        public void LeaveZone(Collider2D collider);
    }
}
