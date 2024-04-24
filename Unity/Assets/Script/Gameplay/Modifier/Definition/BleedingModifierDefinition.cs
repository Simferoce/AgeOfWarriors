using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BleedingModifierDefinition", menuName = "Definition/Modifier/BleedingModifierDefinition")]
    public class BleedingModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, BleedingModifierDefinition>, IAttackSource
        {
            public float DamagePerSeconds { get; set; }
            public int Stacks { get; set; }
            public object Source { get; set; }

            public bool IsMaxed => Stacks >= definition.maxStack;

            private float durationEffect;
            private float lastDamageDealt;
            private float startTime;

            public Modifier(IModifiable modifiable, BleedingModifierDefinition modifierDefinition, object source, float duration) : base(modifiable, modifierDefinition)
            {
                Source = source;
                durationEffect = duration;
                this.With(new CharacterModifierTimeElement(duration));
                Stacks++;
                lastDamageDealt = Time.time;
                startTime = Time.time;
            }

            public override float? GetStack()
            {
                return Stacks;
            }

            public void Increase(float damagePerSeconds)
            {
                if (!IsMaxed)
                {
                    Stacks++;
                    DamagePerSeconds = damagePerSeconds;
                }

                Refresh();
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
                if (modifiable.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                {
                    Attack attack = modifiable.GetCachedComponent<Character>().GenerateAttack(DamagePerSeconds * duration, 0, 0, false, true, attackable, this);
                    attackable.TakeAttack(attack);
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

        [SerializeField] private int maxStack;
    }
}
