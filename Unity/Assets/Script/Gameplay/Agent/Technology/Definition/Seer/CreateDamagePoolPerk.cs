using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CreateDamagePoolPerk", menuName = "Definition/Technology/Seer/CreateDamagePoolPerk")]
    public class CreateDamagePoolPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, CreateDamagePoolPerk>, IProjectileModifier
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;

            public bool HasModifier => currentAttackApplied >= definition.stack;

            public Modifier(IModifiable modifiable, CreateDamagePoolPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                affectedAbility = modifiable.GetCachedComponent<Character>().Abilities.FirstOrDefault(x => x.Definition == definition.affectedAbility);
                affectedAbility.OnAbilityEffectApplied += AffectedAbility_OnAbilityEffectApplied;
            }

            private void AffectedAbility_OnAbilityEffectApplied()
            {
                if (currentAttackApplied < definition.stack)
                    currentAttackApplied++;
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

            public Game.Modifier GetModifier(Projectile projectile)
            {
                currentAttackApplied = 0;
                return new PoolProjectileModifierDefinition.Modifier(projectile.GetCachedComponent<IModifiable>(), definition.projectileModifierDefinition, definition.duration, definition.damage);
            }
        }

        [SerializeField] private int stack;
        [SerializeField] private float duration;
        [SerializeField] private float damage;
        [SerializeField] private PoolProjectileModifierDefinition projectileModifierDefinition;
        [SerializeField] private AbilityDefinition affectedAbility;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
