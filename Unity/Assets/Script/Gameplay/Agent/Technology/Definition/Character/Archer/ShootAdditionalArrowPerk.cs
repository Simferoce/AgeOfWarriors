using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShootAdditionalArrowPerk", menuName = "Definition/Technology/Archer/ShootAdditionalArrowPerk")]
    public class ShootAdditionalArrowPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ShootAdditionalArrowPerk>
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;

            public Modifier(ShootAdditionalArrowPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                affectedAbility = modifiable.Entity.GetCachedComponent<Caster>().Abilities.FirstOrDefault(x => x.Definition == definition.affectedAbility);
                affectedAbility.OnAbilityEffectApplied += AffectedAbility_OnAbilityEffectApplied;
            }

            private void AffectedAbility_OnAbilityEffectApplied()
            {
                if (currentAttackApplied < definition.stack)
                    currentAttackApplied++;

                if (currentAttackApplied >= definition.stack)
                {
                    throw new System.NotImplementedException();
                    //if (affectedAbility.Targets.Count > 1)
                    //{
                    //GameObject gameObject = GameObject.Instantiate(definition.projectilePrefab, definition.origin.GetPosition(affectedAbility), Quaternion.identity);
                    //Projectile projectile = gameObject.GetComponent<Projectile>();
                    //projectile.Ignore = affectedAbility.Targets[0];
                    //ProjectileAngledMovement projectileAngledMovement = projectile.ProjectileMovements.FirstOrDefault(x => x is ProjectileAngledMovement) as ProjectileAngledMovement;
                    //projectileAngledMovement.Angle = 50;

                    //Character character = modifiable.Entity.GetCachedComponent<Character>();
                    //projectile.Initialize(character, affectedAbility.Targets[1], affectedAbility.FactionWhenUsed, projectile.Parameters.ToArray());

                    currentAttackApplied = 0;
                    //}
                }
            }

            public override float? GetStack()
            {
                return currentAttackApplied;
            }

            public override void Dispose()
            {
                base.Dispose();
                affectedAbility.OnAbilityEffectApplied -= AffectedAbility_OnAbilityEffectApplied;
            }
        }

        [SerializeField] private int stack;
        [SerializeField] private AbilityDefinition affectedAbility;
        [SerializeField] private GameObject projectilePrefab;
        //[SerializeReference, SubclassSelector] private ProjectileAbilityEffectOrigin origin;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
