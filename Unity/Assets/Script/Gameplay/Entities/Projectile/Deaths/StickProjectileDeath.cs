using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class StickProjectileDeath : ProjectileDeath
    {
        [SerializeField] private float stopSimulatedDelay = 0.1f;

        private float startAt;

        public override void Start(ProjectileEntity projectile, GameObject collision)
        {
            this.startAt = Time.time;

            projectile.transform.parent = collision.transform;
            GameObject.Destroy(projectile.gameObject, 0.5f);
        }

        public override void Update(ProjectileEntity projectile)
        {
            base.Update(projectile);

            if (projectile.Rigidbody.simulated == true && Time.time - startAt > stopSimulatedDelay)
                projectile.Rigidbody.simulated = false;
        }
    }
}
