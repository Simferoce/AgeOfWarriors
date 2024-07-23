using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReflectDamageModifierDefinition", menuName = "Definition/Modifier/ReflectDamageModifierDefinition")]
    public class ReflectDamageModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ReflectDamageModifierDefinition>, IAttackSource
        {
            private IAttackable attackable;
            private Character character;
            private float damage;

            public Modifier(IModifiable modifiable, ReflectDamageModifierDefinition modifierDefinition, IModifierSource source, float damage) : base(modifiable, modifierDefinition, source)
            {
                this.damage = damage;
                attackable = modifiable.GetCachedComponent<IAttackable>();
                character = modifiable.GetCachedComponent<Character>();
                attackable.OnDamageTaken += Attackable_OnDamageTaken;
            }

            private void Attackable_OnDamageTaken(AttackResult attackResult, IAttackable attackable)
            {
                if (attackResult.Attack.Reflectable)
                {
                    IAttackable target = null;
                    IAttackSource source = attackResult.Attack.AttackSource.Sources.FirstOrDefault(x => x is IComponent component && component.TryGetCachedComponent(out target));
                    if (target != null)
                    {
                        Attack attack = character.GenerateAttack(damage, 0, 0, false, false, false, target, this);
                        target.TakeAttack(attack);
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                attackable.OnDamageTaken -= Attackable_OnDamageTaken;
            }
        }
    }
}
