using UnityEngine;

namespace Game.Character
{
    public partial class CharacterEntity
    {
        public class DeathState : State
        {
            public override bool CanExit => false;

            private float startedAt;

            public DeathState(CharacterEntity character) : base(character)
            {
            }

            protected override void InternalEnter()
            {
                character.Animated.SetTrigger("Dead");
                startedAt = Time.time;
            }

            protected override void InternalExit()
            {

            }

            protected override void InternalUpdate()
            {
                if (Time.time - startedAt > 2)
                    GameObject.Destroy(character.gameObject);
            }
        }
    }
}
