using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageOverTimeModifierDefinition", menuName = "Definition/Modifier/DamageOverTimeModifierDefinition")]
    public class DamageOverTimeModifierDefinition : ModifierDefinition
    {
        public override Game.Modifier Instantiate()
        {
            throw new NotImplementedException();
        }

        public class Modifier : Modifier<Modifier, DamageOverTimeModifierDefinition>
        {
            public float DamagePerSeconds { get; set; }

            private float durationEffect;
            private float lastDamageDealt;
            private float startTime;

            public Modifier(DamageOverTimeModifierDefinition modifierDefinition, float duration, float damageOverTime) : base(modifierDefinition)
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
                        Attack attack = modifiable.Entity.GetCachedComponent<AttackFactory>().Generate(DamagePerSeconds * duration, 0, 0, false, true, false, attackable);
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
    }
}
