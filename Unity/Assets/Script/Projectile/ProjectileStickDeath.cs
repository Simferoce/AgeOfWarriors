using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileStickDeath : ProjectileDeath
    {
        [SerializeField] private float stopSimulatedDelay = 0.1f;

        private Projectile projectile;
        private float startAt;

        public override void Start(Projectile projectile, GameObject collision)
        {
            this.projectile = projectile;

            projectile.transform.parent = collision.transform;
            GameObject.Destroy(projectile.gameObject, 0.5f);
        }

        public override void Update(Projectile projectile)
        {
            base.Update(projectile);

            if (projectile.Rigidbody.simulated == true && Time.time - startAt > stopSimulatedDelay)
                projectile.Rigidbody.simulated = false;
        }
    }
}
