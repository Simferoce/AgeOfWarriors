using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class SelfRotateProjectileMovement : ProjectileMovement
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private bool clockwise = false;

        public override void Update()
        {
            if (projectile.StateValue != ProjectileEntity.State.Dead)
                projectile.transform.rotation *= Quaternion.AngleAxis((clockwise ? -1 : 1) * speed * Time.deltaTime, Vector3.forward);
        }
    }
}
