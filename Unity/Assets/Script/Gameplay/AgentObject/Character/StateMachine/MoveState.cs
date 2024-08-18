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
                character.SetDirection();

                if (character.CanUseAbility())
                {
                    foreach (Ability ability in character.abilities)
                    {
                        if (ability.CanUse())
                        {
                            ability.Use();
                            character.OnAbilityUsed?.Invoke(ability);
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

                CheckStagger();
            }

            private bool CanMove()
            {
                if (character.IsConfused)
                    return false;

                if (character.Speed <= 0)
                    return false;

                if (character.abilities.Any(x => x.IsCasting))
                    return false;

                foreach (IBlock blocker in AgentObject.All.OfType<IBlock>())
                {
                    if (!blocker.IsActive)
                        continue;

                    if (blocker.IsBlocking(character))
                        return false;
                }

                return true;
            }
        }
    }
}
