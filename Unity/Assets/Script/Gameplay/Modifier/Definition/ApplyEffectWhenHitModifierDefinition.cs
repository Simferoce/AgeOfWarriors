using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyEffectWhenHitModifierDefinition", menuName = "Definition/Modifier/ApplyEffectWhenHitModifierDefinition")]
    public class ApplyEffectWhenHitModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyEffectWhenHitModifierDefinition>
        {
            private Instancier instancier;
            private IAttackable attackable;

            public Modifier(IModifiable modifiable, ApplyEffectWhenHitModifierDefinition modifierDefinition, IModifierSource source, Instancier instancier) : base(modifiable, modifierDefinition, source)
            {
                attackable = modifiable.GetCachedComponent<IAttackable>();
                attackable.OnDamageTaken += Attackable_OnDamageTaken;
                this.instancier = instancier;
            }

            private void Attackable_OnDamageTaken(AttackResult attackResult, IAttackable attackee)
            {
                if (attackResult.Attack.OverTime)
                    return;

                IModifiable target = null;
                IAttackSource source = attackResult.Attack.AttackSource.Sources.FirstOrDefault(x => x is IComponent component && component.TryGetCachedComponent(out target));
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
                attackable.OnDamageTaken -= Attackable_OnDamageTaken;
            }
        }
    }
}
