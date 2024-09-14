using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageOverTimeModifierDefinition", menuName = "Definition/Modifier/DamageOverTimeModifierDefinition")]
    public class DamageOverTimeModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DamageOverTimeModifierDefinition>, IAttackSource
        {
            public float DamagePerSeconds { get; set; }

            private float durationEffect;
            private float lastDamageDealt;
            private float startTime;

            public Modifier(ModifierHandler modifiable, DamageOverTimeModifierDefinition modifierDefinition, IModifierSource source, float duration, float damageOverTime) : base(modifiable, modifierDefinition, source)
            {
                DamagePerSeconds = damageOverTime;
                durationEffect = duration;
                this.With(new CharacterModifierTimeElement(duration));
                lastDamageDealt = Time.time;
                startTime = Time.time;
            }

            public override void Update()
            {
                base.Update();

                if (Time.time - lastDamageDealt > 1)
                {
                    DealDamage(Time.time - lastDamageDealt);
                }
            }

            private void DealDamage(float duration)
            {
                if (modifiable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                {
                    Character character = modifiable.Entity.GetCachedComponent<Character>();
                    if (character.Health > 0)
                    {
                        Attack attack = AttackUtility.Generate(character, DamagePerSeconds * duration, 0, 0, false, true, false, attackable, this);
                        attackable.TakeAttack(attack);
                    }
                }

                lastDamageDealt = Time.time;
            }

            public override void Dispose()
            {
                base.Dispose();

                float duration = durationEffect - (lastDamageDealt - startTime);
                if (duration > 0)
                    DealDamage(duration);
            }
        }

        [Serializable]
        public class ModifierInstancier : Instancier<DamageOverTimeModifierDefinition>
        {
            [SerializeField] private float damage;
            [SerializeField] private float duration;

            public override Game.Modifier Instantiate(ModifierHandler modifiable, IModifierSource source)
            {
                Modifier modifier = new DamageOverTimeModifierDefinition.Modifier(modifiable, definition, source, duration, damage);
                return modifier;
            }
        }
    }
}
