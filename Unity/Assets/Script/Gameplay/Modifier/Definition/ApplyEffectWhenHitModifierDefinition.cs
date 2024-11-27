using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyEffectWhenHitModifierDefinition", menuName = "Definition/Modifier/ApplyEffectWhenHitModifierDefinition")]
    public class ApplyEffectWhenHitModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyEffectWhenHitModifierDefinition>
        {
            private Instancier instancier;
            private Attackable attackable;

            public Modifier(ModifierHandler modifiable, ApplyEffectWhenHitModifierDefinition modifierDefinition, IModifierSource source, Instancier instancier) : base(modifiable, modifierDefinition, source)
            {
                attackable = modifiable.Entity.GetCachedComponent<Attackable>();
                attackable.OnAttackTaken += AttackableOnAttackTaken;
                this.instancier = instancier;
            }

            private void AttackableOnAttackTaken(AttackResult attackResult)
            {
                if (attackResult.Attack.OverTime)
                    return;

                ModifierHandler target = attackResult.Attack.AttackFactory.Entity.GetCachedComponent<ModifierHandler>();
                if (target != null && target.TryGetModifier(instancier.Definition, out Game.Modifier modifier))
                {
                    modifier.Refresh();
                }
                else if (target != null)
                {
                    target.AddModifier(instancier.Instantiate(target, Source));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                attackable.OnAttackTaken -= AttackableOnAttackTaken;
            }
        }
    }
}
