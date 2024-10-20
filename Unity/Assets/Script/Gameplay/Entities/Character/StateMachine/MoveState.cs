using Game.Ability;
using Game.Agent;
using Game.Components;
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

            }

            protected override void InternalExit()
            {

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
                    character.rigidbody.linearVelocity = Vector2.right * character.Direction * character.Speed;
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

                foreach (AgentObject agentObject in EntityRepository.Instance.GetByType<AgentObject>())
                {
                    if (agentObject == character)
                        continue;

                    if (!agentObject.TryGetCachedComponent<Blocker>(out Blocker blocker))
                        continue;

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
