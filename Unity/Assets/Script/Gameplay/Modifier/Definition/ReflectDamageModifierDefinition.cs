using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReflectDamageModifierDefinition", menuName = "Definition/Modifier/ReflectDamageModifierDefinition")]
    public class ReflectDamageModifierDefinition : ModifierDefinition
    {
        public override Game.Modifier Instantiate()
        {
            throw new System.NotImplementedException();
        }

        public class Modifier : Modifier<Modifier, ReflectDamageModifierDefinition>
        {
            private Attackable attackable;
            private Character character;
            private float damage;

            public Modifier(ReflectDamageModifierDefinition modifierDefinition, float damage) : base(modifierDefinition)
            {
                this.damage = damage;
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
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
