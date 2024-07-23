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
                if (character.Speed <= 0)
                    return false;

                if (character.abilities.Any(x => x.IsCasting))
                    return false;

                foreach (Blocker blocker in Blocker.All)
                {
                    if (!blocker.IsActive)
                        continue;

                    if (!blocker.Collider.IsTouching(character.GetCachedComponent<Blocker>().Collider))
                        continue;

                    if (blocker.IsBlocking(character))
                        return false;
                }

                return true;
            }
        }
    }
}
