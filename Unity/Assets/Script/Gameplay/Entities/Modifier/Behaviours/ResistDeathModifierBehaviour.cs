using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class ResistDeathModifierBehaviour : ModifierBehaviour
    {
        private Attackable attackable;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            attackable.OnPotentialDeath += OnPotentialDeath;
        }

        private void OnPotentialDeath(AttackData attack, ref bool resist)
        {
            if (resist)
                return;

            resist |= true;
            modifier.Kill();
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.OnPotentialDeath -= OnPotentialDeath;
        }
    }
}