using Game.Character;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Modifier
{
    [Serializable]
    public class OnDummyCreatedModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        private CharacterEntity characterEntity = null;
        private Entity target;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            modifier.GetHierarchy().FirstOrDefault(x => x.TryGetCachedComponent<CharacterEntity>(out characterEntity));

            characterEntity.EventChannelHandler.Subscribe<DummyCreateEventChannel.Event>(OnDummyCreated);
        }

        private void OnDummyCreated(DummyCreateEventChannel.Event evt)
        {
            target = evt.Entity;
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            characterEntity.EventChannelHandler.Unsubscribe<DummyCreateEventChannel.Event>(OnDummyCreated);
        }

        public List<object> GetTargets()
        {
            return new List<object>() { target };
        }
    }
}
