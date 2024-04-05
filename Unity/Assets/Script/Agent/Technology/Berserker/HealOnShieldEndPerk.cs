using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HealOnShieldEndPerk", menuName = "Definition/Technology/Berserker/HealOnShieldEndPerk")]
    public class HealOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float healPerShieldPoint;

            public Modifier(IModifiable modifiable, float healPerShieldPoint) : base(modifiable)
            {
                if (modifiable is IShieldable shieldable)
                {
                    shieldable.OnDeath += Shieldable_OnDeath;
                    shieldable.OnShieldBroken += Shieldable_OnShieldBroken;
                }

                this.healPerShieldPoint = healPerShieldPoint;
            }

            private void Shieldable_OnShieldBroken(Shield shield)
            {
                if (modifiable is not Character character)
                    return;

                float heal = healPerShieldPoint * shield.Remaining;
                if (heal <= 0)
                    return;

                character.Heal(heal);
            }

            private void Shieldable_OnDeath(ITargeteable targeteable)
            {
                if (modifiable is IShieldable shieldable)
                {
                    shieldable.OnDeath -= Shieldable_OnDeath;
                    shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                if (modifiable is IShieldable shieldable)
                {
                    shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
                    shieldable.OnDeath -= Shieldable_OnDeath;
                }
            }
        }

        [SerializeField] private float healPerShieldPoint;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, healPerShieldPoint);
        }
    }
}
