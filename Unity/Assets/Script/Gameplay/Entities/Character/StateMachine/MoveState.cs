using Game.Ability;
using Game.Agent;
using Game.Components;
using System.Linq;
using UnityEngine;

namespace Game.Character
{
    public partial class CharacterEntity
    {
        public class MoveState : State
        {
            public MoveState(CharacterEntity character) : base(character)
            {
            }

            protected override void InternalEnter()
            {
                if (character.Animated.GetCurrent() != "Move")
                    character.Animated.Play("Move");
            }

            protected override void InternalExit()
            {
                character.rigidbody.linearVelocity = Vector2.zero;
            }

            protected override void InternalUpdate()
            {
                character.RefreshDirection();
                if (!character.IsDead && character.Health > 0 && character.GetCachedComponent<Caster>().IsCasting == false)
                {
                    foreach (AbilityEntity ability in character.GetCachedComponent<Caster>().Abilities)
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
                    character.Animated.SetFloat("SpeedRatio", 1, 0.25f);
                    character.rigidbody.linearVelocity = Vector2.right * character.GetCachedComponent<AgentIdentity>().Direction * character.Speed;
                }
                else
                {
                    character.Animated.SetFloat("SpeedRatio", 0f, 0.25f);
                    character.rigidbody.linearVelocity = Vector2.zero;
                }

                CheckStagger();
            }

            private bool CanMove()
            {
                if (character.IsConfused)
                    return false;

                if (character.Speed <= 0)
                    return false;

                if (character.GetCachedComponent<Caster>().IsCasting)
                    return false;

                foreach (Entity entity in Entity.All.OfType<Entity>())
                {
                    if (entity == character)
                        continue;

                    if (!entity.TryGetCachedComponent<Blocker>(out Blocker blocker))
                        continue;

                    if (!blocker.enabled)
                        continue;

                    if (blocker.IsBlocking(character))
                        return false;
                }

                return true;
            }
        }
    }
}
