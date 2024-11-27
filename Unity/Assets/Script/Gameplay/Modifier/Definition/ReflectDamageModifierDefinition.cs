using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReflectDamageModifierDefinition", menuName = "Definition/Modifier/ReflectDamageModifierDefinition")]
    public class ReflectDamageModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ReflectDamageModifierDefinition>
        {
            private Attackable attackable;
            private Character character;
            private float damage;

            public Modifier(ModifierHandler modifiable, ReflectDamageModifierDefinition modifierDefinition, IModifierSource source, float damage) : base(modifiable, modifierDefinition, source)
            {
                this.damage = damage;
                attackable = modifiable.Entity.GetCachedComponent<Attackable>();
                character = modifiable.Entity.GetCachedComponent<Character>();
                attackable.OnAttackTaken += Attackable_OnDamageTaken;
            }

            private void Attackable_OnDamageTaken(AttackResult attackResult)
            {
                if (attackResult.Attack.Reflectable)
                {
                    Attackable target = null;
                    if (target != null)
                    {
                        Attack attack = modifiable.Entity.GetCachedComponent<AttackFactory>().Generate(damage, 0, 0, false, false, false, target);
                        target.TakeAttack(attack);
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                attackable.OnAttackTaken -= Attackable_OnDamageTaken;
            }
        }
    }
}
