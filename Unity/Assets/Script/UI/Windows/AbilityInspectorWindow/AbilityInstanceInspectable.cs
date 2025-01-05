using Game.Ability;

namespace Game.UI.Windows
{
    public class AbilityInstanceInspectable : IAbilityInspectable
    {
        private AbilityEntity ability;

        public AbilityInstanceInspectable(AbilityEntity ability)
        {
            this.ability = ability;
        }

        public float GetCooldown()
        {
            return ability.TotalCooldown;
        }

        public string GetDescription()
        {
            return ability.GetDefinition().ParseDescription(ability);
        }

        public string GetTitle()
        {
            return ability.GetDefinition().Title;
        }
    }
}
