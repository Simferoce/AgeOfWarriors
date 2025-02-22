using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class AlignInShootDirection : ProjectileMovement
    {
        private float direction;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            ProjectileParameter<float> projectileParameter = projectile.Parameters.OfType<ProjectileParameter<float>>().FirstOrDefault(x => x.Name == "direction");
            if (projectileParameter == null)
            {
                Debug.LogError($"Could not find a parameter with the name \"direction\".");
                return;
            }

            direction = projectileParameter.GetValue();
        }

        public override void Update()
        {
            projectile.transform.right = Vector3.right * direction;
        }
    }
}
