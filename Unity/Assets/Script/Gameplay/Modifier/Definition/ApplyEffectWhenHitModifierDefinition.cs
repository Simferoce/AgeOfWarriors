using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyEffectWhenHitModifierDefinition", menuName = "Definition/Modifier/ApplyEffectWhenHitModifierDefinition")]
    public class ApplyEffectWhenHitModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyEffectWhenHitModifierDefinition>
        {
            private Attackable attackable;

            public Modifier(ApplyEffectWhenHitModifierDefinition modifierDefinition) : base(modifierDefinition)
            {
            }

            //public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            //{
            //    base.Initialize(modifiable, source, parameters);
            //    attackable = modifiable.Entity.GetCachedComponent<Attackable>();
            //    attackable.OnAttackTaken += AttackableOnAttackTaken;
            //}

            //private void AttackableOnAttackTaken(AttackResult attackResult)
            //{
            //    if (attackResult.Attack.OverTime)
            //        return;

            //    ModifierHandler target = attackResult.Attack.AttackFactory.Entity.GetCachedComponent<ModifierHandler>();
            //    if (target != null && target.TryGetModifier(instancier.Definition, out Game.Modifier modifier))
            //    {
            //        modifier.Refresh();
            //    }
            //    else if (target != null)
            //    {
            //        target.AddModifier(instancier.Instantiate(target, Source));
            //    }
            //}

            //public override void Dispose()
            //{
            //    base.Dispose();
            //    attackable.OnAttackTaken -= AttackableOnAttackTaken;
            //}
        }

        public override Game.Modifier Instantiate()
        {
            throw new System.NotImplementedException();
        }

    }
}
