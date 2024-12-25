using System;

namespace Game.Modifier
{
    [Serializable]
    public class OnDestroyModifierTrigger : ModifierTrigger
    {
        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifier.OnRemoved += OnRemoved;
        }

        private void OnRemoved(ModifierEntity modifier)
        {
            modifier.OnRemoved -= OnRemoved;
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            modifier.OnRemoved -= OnRemoved;
        }
    }
}
