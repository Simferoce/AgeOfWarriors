using Game;
using System;
using UnityEngine;

public partial class Attackable : MonoBehaviour
{
    public struct Input
    {
        public Attackable Attacked { get; set; }
        public float CurrentHealth { get; set; }
        public float Defense { get; set; }
        public float IncreaseDamageTaken { get; set; }
        public float RangedDamageReduction { get; set; }
        //public List<Shield> Shields { get; set; }

        public bool CanResistDeath { get; set; }

        public Input(Attackable attacked, float currentHealth = 0, float defense = 0, float increaseDamageTaken = 0, float rangedDamageReduction = 0, /*List<Shield> shields = null,*/ bool canResistDeath = false)
        {
            Attacked = attacked;
            CurrentHealth = currentHealth;
            Defense = defense;
            IncreaseDamageTaken = increaseDamageTaken;
            RangedDamageReduction = rangedDamageReduction;
            //Shields = shields;
            CanResistDeath = canResistDeath;
        }
    }

    public Entity Entity { get; set; }
    public float LastTimeAttacked { get; private set; }
    public event Action<AttackResult, Attackable> OnDamageTaken;

    private void Awake()
    {
        Entity = GetComponentInParent<Entity>();
    }

    public void TakeAttack(Attack attack)
    {
        float currentHealth = Entity.GetCachedComponent<StatisticIndex>().SelfByDefinition<float>(StatisticRepository.Health);
        float currentDefense = Entity.GetCachedComponent<StatisticIndex>().SelfByDefinition<float>(StatisticRepository.Defense);
        float increaseDamageTaken = 0f;//Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Sum(x => x.IncreaseDamageTaken ?? 0);
        float rangedDamageReduction = 0f;//Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Sum(x => x.RangedDamageReduction ?? 0);
        //List<Shield> shields = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().OfType<ShieldModifierDefinition.Shield>().ToList();
        //bool canResistDeath = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());

        float damage = attack.Damage;
        float resultingDefense = currentDefense - attack.ArmorPenetration;

        float damageTakenModifier = -increaseDamageTaken;
        if (attack.Flags.HasFlag(Attack.Flag.Ranged))
            damageTakenModifier += rangedDamageReduction;

        damageTakenModifier = Mathf.Clamp(1 - damageTakenModifier, 0.35f, 1.65f);
        damage *= damageTakenModifier;

        float damageRemaining = damage;
        if (!attack.Flags.HasFlag(Attack.Flag.OverTime))
            damageRemaining *= (1 / (1 + resultingDefense * 0.1f));

        float defenseDamagePrevented = damage - damageRemaining;

        //float damageBeforeAbsortion = damageRemaining;
        //for (int i = 0; i < shields.Count; i++)
        //    shields[i].Absorb(damageRemaining, out damageRemaining);

        //float damageAbsorbed = damageBeforeAbsortion - damageRemaining;

        bool resistedDeath = false;
        //if (canResistDeath && currentHealth - damageRemaining <= 0)
        //{
        //    resistedDeath = true;
        //    damageRemaining = currentHealth - 0.01f;
        //}

        AttackResult attackResult = new AttackResult(attack, damageRemaining, defenseDamagePrevented, damageRemaining >= currentHealth, this, resistedDeath);
        attack.Source.NotifyAttackResult(attackResult);

        OnDamageTaken?.Invoke(attackResult, this);
        LastTimeAttacked = Time.time;
    }

    //public float Absorb(float damageRemaining, List<Shield> shields)
    //{
    //    for (int i = 0; i < shields.Count; i++)
    //        shields[i].Absorb(damageRemaining, out damageRemaining);

    //    return damageRemaining;
    //}
}

