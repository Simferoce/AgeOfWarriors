using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HunterMarkModifierDefinition", menuName = "Definition/Modifier/HunterMarkModifierDefinition")]
    public class HunterMarkModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier>, IAttackSource
        {
            private float damage;
            private IAttackable attackable;
            public HuntersMarkAbilityEffect Source { get; set; }

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, HuntersMarkAbilityEffect source, float damage, IAttackable attackable) : base(modifiable, modifierDefinition)
            {
                this.Source = source;
                this.attackable = attackable;
                this.damage = damage;
                attackable.OnDamageTaken += OnAttackableDamageTaken; ;
            }

            private void OnAttackableDamageTaken(Attack attack, IAttackable attackable)
            {
                if (attack.AttackSource.Sources.Contains(this))
                    return;

                AttackSource source = attack.AttackSource.Clone();
                source.Sources.Add(this);
                attackable.TakeAttack(new Attack(source, damage, 0f, 0f));
            }

            public override void Dispose()
            {
                base.Dispose();

                attackable.OnDamageTaken -= OnAttackableDamageTaken;
            }
        }
    }
}
