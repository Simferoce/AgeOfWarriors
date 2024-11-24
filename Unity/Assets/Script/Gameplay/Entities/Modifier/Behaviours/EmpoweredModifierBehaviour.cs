using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class EmpoweredModifierBehaviour : ModifierBehaviour
    {
        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifier.Target.Entity.GetCachedComponent<AttackFactory>().OnGenerateAttack += EmpoweredModifierBehaviourOnGenerateAttack;
        }

        private void EmpoweredModifierBehaviourOnGenerateAttack(AttackData attackData)
        {
            attackData.Flags |= AttackData.Flag.Empowered;
            modifier.Kill();
        }

        public override void Dispose()
        {
            base.Dispose();

            modifier.Target.Entity.GetCachedComponent<AttackFactory>().OnGenerateAttack -= EmpoweredModifierBehaviourOnGenerateAttack;
        }
    }
}
