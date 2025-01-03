using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class AnimationProjectileDeath : ProjectileDeath
    {
        [SerializeField] private string trigger;
        [SerializeField] private float duration;

        public override void Start(ProjectileEntity projectile, GameObject collision)
        {
            projectile.GetComponentInChildren<Animator>().SetTrigger(trigger);
            projectile.Rigidbody.simulated = false;
            GameObject.Destroy(projectile.gameObject, duration);
        }
    }
}