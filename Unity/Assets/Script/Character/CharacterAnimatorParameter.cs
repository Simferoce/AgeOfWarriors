using System;
using UnityEngine;

namespace Game
{
    public static class CharacterAnimatorParameter
    {
        public enum Parameter
        {
            SpeedRatio,
            Ability,
            SpecialAbility1,
            SpecialAbility2,
            Dead,
            Stagger,
            EndStagger
        }

        public static int Convert(this Parameter trigger)
        {
            return trigger switch
            {
                Parameter.SpeedRatio => Animator.StringToHash("SpeedRatio"),
                Parameter.Ability => Animator.StringToHash("Ability"),
                Parameter.SpecialAbility1 => Animator.StringToHash("SpecialAbility1"),
                Parameter.SpecialAbility2 => Animator.StringToHash("SpecialAbility2"),
                Parameter.Dead => Animator.StringToHash("Dead"),
                Parameter.Stagger => Animator.StringToHash("Stagger"),
                Parameter.EndStagger => Animator.StringToHash("EndStagger"),
                _ => throw new NotImplementedException()
            };
        }
    }
}
