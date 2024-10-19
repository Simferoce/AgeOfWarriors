using UnityEngine;

namespace Game.Character
{
    public partial class CharacterEntity
    {
        public class DeathState : State
        {
            public override bool CanExit => false;

            public DeathState(CharacterEntity character) : base(character)
            {
            }

            protected override void InternalEnter()
            {
                character.Animated.SetTrigger("Dead");
                GameObject.Destroy(character.gameObject, 2);
            }

            protected override void InternalExit()
            {

            }

            protected override void InternalUpdate()
            {

            }
        }
    }
}
