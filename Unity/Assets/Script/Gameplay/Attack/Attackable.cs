using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Attackable : Entity, IAttackable, IShieldable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;
        public event IShieldable.ShieldBroken OnShieldBroken;
        public event Action<IShieldable> OnShieldableDestroyed;

        public List<Shield> Shields { get => shields; }

        private List<Shield> shields = new List<Shield>();
        private IAttackableOwner owner;

        private void Awake()
        {
            owner = GetCachedComponent<IAttackableOwner>();
        }

        public void TakeAttack(Attack attack)
        {
            if (!owner.IsActive)
                return;

            if (owner.IsInvulnerable)
                return;

            float damageRemaining = DefenseFormulaDefinition.Instance.ParseDamage(attack.Damage, Mathf.Max(0, owner.Defense - attack.ArmorPenetration));
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

            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attack, damageRemaining, owner.Health <= 0);

            Debug.Log($"{this.name} took {damageRemaining} (reduced by {attack.Damage - damageRemaining}) from {attack.AttackSource.Sources[^1]}");
            OnDamageTaken?.Invoke(attack, this);

            if (owner.Health <= 0 && !owner.IsDead)
                owner.Death();
        }

        public void AddShield(Shield shield)
        {
            shields.Add(shield);
        }

        public void Update()
        {
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                Shield shield = shields[i];
                if (shield.Update())
                {
                    shields.Remove(shield);
                    OnShieldBroken?.Invoke(shield);
                }
            }
        }

        public float Absorb(float damageRemaining)
        {
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                Shield shield = shields[i];
                if (!shield.Absorb(damageRemaining, out damageRemaining))
                {
                    OnShieldBroken?.Invoke(shield);
                    shields.RemoveAt(i);
                }
            }

            return damageRemaining;
        }

        public void OnDestroy()
        {
            OnShieldableDestroyed?.Invoke(this);
        }
    }
}
