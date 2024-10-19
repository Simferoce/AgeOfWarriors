using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class BubbleUpProjectileDeath : ProjectileDeath
    {
        [SerializeField] private float duration = 3f;
        [SerializeField] private float scale = 2f;
        [SerializeField] private float stopSimulatedDelay = 0.1f;

        private ProjectileEntity projectile;
        private float startAt;
        private Vector3 originalScale;
        private SpriteRenderer[] spriteRenderers;

        public override void Start(ProjectileEntity projectile, GameObject collision)
        {
            spriteRenderers = projectile.GetComponentsInChildren<SpriteRenderer>();
            originalScale = projectile.transform.localScale;
            startAt = Time.time;

            this.projectile = projectile;

            GameObject.Destroy(projectile.gameObject, duration);
        }

        public override void Update(ProjectileEntity projectile)
        {
            base.Update(projectile);

            if (projectile.Rigidbody.simulated == true && Time.time - startAt > stopSimulatedDelay)
                projectile.Rigidbody.simulated = false;

            if (Time.time - startAt > stopSimulatedDelay)
            {
                float delta = ((Time.time - startAt) - stopSimulatedDelay) / (duration - stopSimulatedDelay);

                projectile.transform.localScale = Vector3.Lerp(originalScale, new Vector3(scale, scale, scale), delta);

                Color color = Color.white;
                color.a = 1 - delta;

                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.color = color;
                }
            }
        }
    }
}
