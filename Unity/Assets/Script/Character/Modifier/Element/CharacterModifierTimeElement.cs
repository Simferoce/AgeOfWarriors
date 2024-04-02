using UnityEngine;

namespace Game
{
    public class CharacterModifierTimeElement : ModifierElement
    {
        private float duration = 0f;
        private float startedAt = 0f;

        public CharacterModifierTimeElement(float duration)
        {
            this.duration = duration;
        }

        public override void Initialize()
        {
            startedAt = Time.time;
        }

        public override void Refresh()
        {
            startedAt = Time.time;
        }

        public override bool Update()
        {
            return Time.time - startedAt > duration;
        }
    }
}
