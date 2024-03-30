using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HuntersMarkAbilityEffect : ApplyModifierAbilityEffect
    {
        [SerializeField] private float damage;
        [SerializeField] private float duration;

        public class HuntersMark : Modifier<HuntersMark>, IAttackSource
        {
            private float damage;
            private IAttackable attackable;
            public HuntersMarkAbilityEffect Source { get; set; }

            public HuntersMark(HuntersMarkAbilityEffect source, float damage, IModifiable modifiable, IAttackable attackable) : base(modifiable)
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
                attackable.TakeAttack(new Attack(source, damage, 0f));
            }

            public override void Dispose()
            {
                base.Dispose();

                attackable.OnDamageTaken -= OnAttackableDamageTaken;
            }
        }

        public override void Apply()
        {
            List<IAttackable> attackable = character.GetTargets();
            IModifiable modifiable = attackable.FirstOrDefault(x => x is IModifiable) as IModifiable;

            if (modifiable != null)
            {
                HuntersMark huntersMark = (HuntersMark)modifiable.GetModifiers().FirstOrDefault(x => x is HuntersMark huntersMark && huntersMark.Source == this);

                if (huntersMark != null)
                {
                    huntersMark.Refresh();
                }
                else
                {
                    huntersMark = new HuntersMark(this, damage, modifiable, modifiable as IAttackable)
                        .With(new CharacterModifierTimeElement(duration));

                    modifiable.AddModifier(huntersMark);
                }
            }
        }
    }
}
