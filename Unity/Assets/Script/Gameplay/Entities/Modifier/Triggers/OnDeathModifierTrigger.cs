using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnDeathModifierTrigger : ModifierTrigger
    {
        [SerializeReference, SubclassSelector] private ModifierTargetFilter targetFilter;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            DeathEventChannel.Global.Subscribe(OnDeath);

            if (targetFilter != null)
                targetFilter.Initialize(modifier);
        }

        private void OnDeath(DeathEventChannel.Event evt)
        {
            if (targetFilter == null && evt.Entity == modifier.Target.Entity)
                Trigger();

            if (targetFilter != null && targetFilter.Execute(evt.Entity))
                Trigger();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Dispose()
        {
            base.Dispose();
            DeathEventChannel.Global.Unsubscribe(OnDeath);
        }
    }
}
