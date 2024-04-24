using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Attackable : Entity, IAttackable, IShieldable
    {
        public event Action<AttackResult, IAttackable> OnDamageTaken;
        public event Action<IShieldable> OnShieldableDestroyed;

        private IAttackableOwner owner;
        private IModifiable modifiable;

        private void Awake()
        {
            owner = GetCachedComponent<IAttackableOwner>();
            modifiable = owner.GetCachedComponent<IModifiable>();
        }

        public void TakeAttack(Attack attack)
        {
            if (!owner.IsActive)
                return;

            if (owner.IsInvulnerable)
                return;

            float damage = attack.Damage;
            float defense = owner.Defense - attack.ArmorPenetration;

            if (attack.Ranged)
            {
                damage *= Mathf.Clamp(1 - modifiable.GetModifiers().Sum(x => x.RangedDamageReduction ?? 0), 0.35f, 1f);
            }

            float damageRemaining = damage;
            if (!attack.OverTime)
            {
                damageRemaining *= (1 / (1 + (defense) * 0.1f));
            }

            float defenseDamagePrevented = damage - damageRemaining;

            damageRemaining = Absorb(damageRemaining);

            owner.Health -= damageRemaining;

            if (owner.Health <= 0)
            {
                ResistKillingBlowPerk.Modifier modifier = (ResistKillingBlowPerk.Modifier)owner.GetCachedComponent<IModifiable>().GetModifiers().FirstOrDefault(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());
                if (modifier != null)
                {
                    modifier.ResistKillingBlow();
                    owner.Health = 0.001f;
                }
            }

            AttackResult attackResult = new AttackResult(attack, damageRemaining, defenseDamagePrevented, owner.Health <= 0, GetCachedComponent<Attackable>());
            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attackResult);

            Debug.Log($"{this.name} took {damageRemaining} (reduced by {attack.Damage - damageRemaining}) from {attack.AttackSource.Sources[^1]}");

            OnDamageTaken?.Invoke(attackResult, this);

            if (owner.Health <= 0 && !owner.IsDead)
                owner.Death();
        }


        public float Absorb(float damageRemaining)
        {
            List<ShieldModifierDefinition.Shield> shields = modifiable.GetModifiers().OfType<ShieldModifierDefinition.Shield>().ToList();
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                ShieldModifierDefinition.Shield shield = shields[i];
                if (!shield.Absorb(damageRemaining, out damageRemaining))
                    modifiable.RemoveModifier(shield);
            }

            return damageRemaining;
        }

        public void OnDestroy()
        {
            OnShieldableDestroyed?.Invoke(this);
        }
    }
}
