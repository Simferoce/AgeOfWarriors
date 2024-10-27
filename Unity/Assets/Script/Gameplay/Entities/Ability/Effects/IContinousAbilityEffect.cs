using Game.Components;

namespace Game.Ability
{
    public interface IContinousAbilityEffect
    {
        public bool Update(Caster character);
    }
}
