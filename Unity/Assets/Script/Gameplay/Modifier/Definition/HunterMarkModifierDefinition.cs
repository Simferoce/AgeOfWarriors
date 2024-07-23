using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HunterMarkModifierDefinition", menuName = "Definition/Modifier/HunterMarkModifierDefinition")]
    public class HunterMarkModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, HunterMarkModifierDefinition>, IAttackSource
        {
            private float damage;
            private IAttackable attackable;
            public HuntersMarkAbilityEffect Source { get; set; }

            public Modifier(IModifiable modifiable, HunterMarkModifierDefinition modifierDefinition, HuntersMarkAbilityEffect source, float damage, IAttackable attackable, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.Source = source;
                this.attackable = attackable;
                this.damage = damage;
                attackable.OnDamageTaken += OnAttackableDamageTaken; ;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, damage);
            }

            private void OnAttackableDamageTaken(AttackResult attack, IAttackable attackable)
            {
                if (attack.Attack.AttackSource.Sources.Contains(this))
                    return;

                AttackSource source = attack.Attack.AttackSource.Clone();
                source.Sources.Add(this);
                attackable.TakeAttack(new Attack(source, damage, 0f, 0f, false, false, false, false));
            }

            public override void Dispose()
            {
                base.Dispose();

                attackable.OnDamageTaken -= OnAttackableDamageTaken;
            }
        }
    }
}
