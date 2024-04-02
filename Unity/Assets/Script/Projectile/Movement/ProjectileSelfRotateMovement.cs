using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileSelfRotateMovement : ProjectileMovement
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private bool clockwise = false;

        public override void Update()
        {
            if (projectile.StateValue != Projectile.State.Dead)
                projectile.transform.rotation *= Quaternion.AngleAxis((clockwise ? -1 : 1) * speed * Time.deltaTime, Vector3.forward);
        }
    }
}
