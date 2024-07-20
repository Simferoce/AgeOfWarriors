using Game;
using System;
using UnityEngine;

[Serializable]
public class ProjectileAnimationDeath : ProjectileDeath
{
    [SerializeField] private string trigger;
    [SerializeField] private float duration;

    public override void Start(Projectile projectile, GameObject collision)
    {
        projectile.GetComponentInChildren<Animator>().SetTrigger(trigger);
        GameObject.Destroy(projectile.gameObject, duration);
    }
}

