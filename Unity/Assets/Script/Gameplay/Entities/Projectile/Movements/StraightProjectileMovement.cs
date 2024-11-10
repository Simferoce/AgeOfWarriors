using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class StraightProjectileMovement : ProjectileMovement
    {
        [SerializeField] private float speed;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            ProjectileParameter<float> projectileParameter = projectile.Parameters.OfType<ProjectileParameter<float>>().FirstOrDefault(x => x.Name == "direction");
            if (projectileParameter == null)
            {
                Debug.LogError($"Could not find a parameter with the name \"direction\".");
                return;
            }

            Vector3 velocity = Vector3.right * projectileParameter.GetValue() * speed;
            projectile.Rigidbody.linearVelocity = velocity;
            projectile.transform.right = projectile.Rigidbody.linearVelocity;
        }

        public override void Update()
        {

        }
    }
}
