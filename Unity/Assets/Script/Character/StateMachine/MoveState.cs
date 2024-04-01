using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Character
    {
        public class MoveState : State
        {
            public MoveState(Character character) : base(character)
            {
            }

            protected override void InternalEnter()
            {

            }

            protected override void InternalExit()
            {

            }

            protected override void InternalUpdate()
            {
                if (character.CanUseAbility())
                {
                    foreach (CharacterAbility ability in character.abilities)
                    {
                        if (ability.CanUse())
                        {
                            ability.Use();
                            break;
                        }
                    }
                }

                if (CanMove())
                {
                    character.CharacterAnimator.SetFloat(CharacterAnimatorParameter.Parameter.SpeedRatio, 1, 0.25f);
                    character.rigidbody.MovePosition(character.rigidbody.position + Vector2.right * character.Direction * character.Speed * Time.deltaTime);
                }
                else
                {
                    character.CharacterAnimator.SetFloat(CharacterAnimatorParameter.Parameter.SpeedRatio, 0f, 0.25f);
                }
            }

            private bool CanMove()
            {
                if (character.abilities.Any(x => x.IsCasting))
                    return false;

                foreach (IBlocker blocker in AgentObject.All.OfType<IBlocker>())
                {
                    if (!blocker.IsActive)
                        continue;

                    if (!blocker.Collider.IsTouching(character.Collider))
                        continue;

                    if (!blocker.IsBlocking(character.Faction))
                        continue;

                    if (blocker.Faction != character.Faction)
                        return false;

                    if (blocker.Priority < character.Priority)
                        return false;
                }

                return true;
            }
        }
    }
}
