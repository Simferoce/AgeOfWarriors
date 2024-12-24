using Game.Components;
using System;
using System.Linq;

namespace Game.Modifier
{
    [Serializable]
    public class OnAbilityUseTrigger : ModifierTrigger
    {
        private Caster caster = null;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            modifier.GetHierarchy().FirstOrDefault(x => x.TryGetCachedComponent<Caster>(out caster));

            caster.OnAbilityUsed += Caster_OnAbilityUsed;
        }

        private void Caster_OnAbilityUsed(Ability.AbilityEntity obj)
        {
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            caster.OnAbilityUsed -= Caster_OnAbilityUsed;
        }
    }
}
