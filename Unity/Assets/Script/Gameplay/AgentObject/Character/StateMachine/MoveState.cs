﻿using UnityEngine;

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
                character.RefreshDirection();

                if (!character.IsDead && character.Health > 0)
                {
                    foreach (Ability ability in character.GetCachedComponent<Caster>().Abilities)
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

                foreach (AgentObject agentObject in AgentObject.All)
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
