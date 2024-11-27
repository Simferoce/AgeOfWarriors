using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PoolProjectileModifierDefinition", menuName = "Definition/Modifier/Projectile/PoolProjectileModifierDefinition")]
    public class PoolProjectileModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, PoolProjectileModifierDefinition>
        {
            private Projectile projectile;
            private float duration;
            private float damage;

            public Modifier(ModifierHandler modifiable, PoolProjectileModifierDefinition modifierDefinition, float duration, float damage, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
                this.projectile = modifiable.Entity.GetCachedComponent<Projectile>();
                projectile.OnImpacted += Projectile_OnImpacted;

                this.damage = damage;
                this.duration = duration;
            }

            private void Projectile_OnImpacted(System.Collections.Generic.List<ITargeteable> targeteables)
            {
                Vector3 position = Lane.Instance.Project(projectile.transform.position);
                Pool pool = GameObject.Instantiate(definition.prefab, position, Quaternion.identity).GetComponent<Pool>();
                pool.Duration = duration;
                pool.GetEffect<PeriodicDamagePoolEffect>().Damage = damage;
                pool.Spawn(projectile.AgentObject.Agent, 0, projectile.AgentObject.Direction);
                pool.Initialize();
            }
        }

        [SerializeField] private GameObject prefab;
    }
}
