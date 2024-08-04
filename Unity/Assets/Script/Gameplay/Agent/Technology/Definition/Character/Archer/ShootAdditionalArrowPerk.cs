using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShootAdditionalArrowPerk", menuName = "Definition/Technology/Archer/ShootAdditionalArrowPerk")]
    public class ShootAdditionalArrowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ShootAdditionalArrowPerk>
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;

            public Modifier(IModifiable modifiable, ShootAdditionalArrowPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                affectedAbility = modifiable.GetCachedComponent<Character>().Abilities.FirstOrDefault(x => x.Definition == definition.affectedAbility);
                affectedAbility.OnAbilityEffectApplied += AffectedAbility_OnAbilityEffectApplied;
            }

            private void AffectedAbility_OnAbilityEffectApplied()
            {
                if (currentAttackApplied < definition.stack)
                    currentAttackApplied++;

                if (currentAttackApplied >= definition.stack)
                {
                    if (affectedAbility.Targets.Count > 1)
                    {
                        GameObject gameObject = GameObject.Instantiate(definition.projectilePrefab, definition.origin.GetPosition(affectedAbility), Quaternion.identity);
                        Projectile projectile = gameObject.GetComponent<Projectile>();
                        projectile.Ignore = affectedAbility.Targets[0];
                        ProjectileAngledMovement projectileAngledMovement = projectile.ProjectileMovements.FirstOrDefault(x => x is ProjectileAngledMovement) as ProjectileAngledMovement;
                        projectileAngledMovement.Angle = 50;

                        Character character = modifiable.GetCachedComponent<Character>();
                        projectile.Initialize(character, projectile.Context, affectedAbility.Targets[1], affectedAbility.FactionWhenUsed);

                        currentAttackApplied = 0;
                    }
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
        [SerializeReference, SubclassSelector] private ProjectileAbilityEffectOrigin origin;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }
    }
}
