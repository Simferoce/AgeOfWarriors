using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReflectDamageModifierDefinition", menuName = "Definition/Modifier/ReflectDamageModifierDefinition")]
    public class ReflectDamageModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ReflectDamageModifierDefinition>, IAttackSource
        {
            private Attackable attackable;
            private Character character;
            private float damage;

            public Modifier(ModifierHandler modifiable, ReflectDamageModifierDefinition modifierDefinition, IModifierSource source, float damage) : base(modifiable, modifierDefinition, source)
            {
                this.damage = damage;
                attackable = modifiable.Entity.GetCachedComponent<Attackable>();
                character = modifiable.Entity.GetCachedComponent<Character>();
                attackable.OnDamageTaken += Attackable_OnDamageTaken;
            }

            private void Attackable_OnDamageTaken(AttackResult attackResult, Attackable attackable)
            {
                if (attackResult.Attack.Reflectable)
                {
                    Attackable target = null;
                    IAttackSource source = attackResult.Attack.AttackSource.Sources.FirstOrDefault(x => x is IComponent component && component.Entity.TryGetCachedComponent(out target));
                    if (target != null)
                    {
                        Attack attack = AttackUtility.Generate(character, damage, 0, 0, false, false, false, target, this);
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
