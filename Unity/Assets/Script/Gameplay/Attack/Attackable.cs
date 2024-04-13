using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Attackable
    {
        public event Action<Attack, IAttackable> OnDamageTaken;

        private AgentObject owner;

        public ShieldHandler ShieldHandler { get; } = new ShieldHandler();

        public void Initialize(AgentObject owner)
        {
            this.owner = owner;
            ShieldHandler.Initialize(owner);
        }

        public void TakeAttack(Attack attack)
        {
            if (owner.IsDead)
                return;

            if (owner.IsInvulnerable)
                return;

            float damageRemaining = DefenseFormulaDefinition.Instance.ParseDamage(attack.Damage, Mathf.Max(0, owner.Defense - attack.ArmorPenetration));
            ShieldHandler.Absorb(damageRemaining);

            owner.Health -= damageRemaining;

            if (owner.Health <= 0)
            {
                ResistKillingBlowPerk.Modifier modifier = (ResistKillingBlowPerk.Modifier)owner.GetModifiers().FirstOrDefault(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());
                if (modifier != null)
                {
                    modifier.ResistKillingBlow();
                    owner.Health = 0.001f;
                }
            }

            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attack, damageRemaining, owner.Health <= 0);

            Debug.Log($"{owner.name} took {damageRemaining} (reduced by {attack.Damage - damageRemaining}) from {attack.AttackSource.Sources[^1]}");
            OnDamageTaken?.Invoke(attack, owner);

            if (owner.Health <= 0 && !owner.IsDead)
                owner.Death();
        }

        public void AddShield(Shield shield)
        {
            ShieldHandler.AddShield(shield);
        }

        public void Update()
        {
            ShieldHandler.Update();
        }

        public void OnDestroy()
        {
            ShieldHandler.OnDestroy();
        }
    }
}
