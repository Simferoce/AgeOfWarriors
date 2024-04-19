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

            private void OnAttackableDamageTaken(AttackResult attack, IAttackable attackable)
            {
                if (attack.Attack.AttackSource.Sources.Contains(this))
                    return;

                AttackSource source = attack.Attack.AttackSource.Clone();
                source.Sources.Add(this);
                attackable.TakeAttack(new Attack(source, damage, 0f, 0f));
            }

            public override void Dispose()
            {
                base.Dispose();

                attackable.OnDamageTaken -= OnAttackableDamageTaken;
            }

            public override bool TryGetValue<T>(StatisticDefinition definition, out T value)
            {
                if (definition == StatisticDefinition.Damage)
                {
                    value = (T)(object)damage;
                    return true;
                }

                return base.TryGetValue(definition, out value);
            }
        }
    }
}
