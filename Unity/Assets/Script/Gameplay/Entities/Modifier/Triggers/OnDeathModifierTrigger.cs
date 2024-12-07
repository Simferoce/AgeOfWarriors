using Game.EventChannel;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class OnDeathModifierTrigger : ModifierTrigger
    {
        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            DeathEventChannel.Instance.Susbribe(OnDeath);
        }

        private void OnDeath(DeathEventChannel.Event evt)
        {
            if (evt.Entity == modifier.Target.Entity)
                Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            DeathEventChannel.Instance.Unsubcribe(OnDeath);
        }
    }
}
