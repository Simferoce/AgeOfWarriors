using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InspiringPresenceAbilityEffect : AbilityEffect, ILingeringAbilityEffect
    {
        private class InspiringPresenceBuff : CharacterModifier
        {
            public InspiringPresenceAbilityEffect Source { get; }
            public float Duration { get; private set; }
            public override float? Defense => defense;

            private float defense;
            private float startedAt;

            public InspiringPresenceBuff(Character character, float defense, InspiringPresenceAbilityEffect source, float duration)
                : base(character)
            {
                this.defense = defense;
                this.Source = source;
                Duration = duration;
                startedAt = Time.time;
            }

            public override void Update()
            {
                if (Time.time - startedAt > Duration)
                    character.CharacterModifierHandler.Modifiers.Remove(this);
            }

            public override void Refresh()
            {
                startedAt = Time.time;
            }
        }

        [SerializeField] private float duration = 5f;
        [SerializeField] private float area = 2f;
        [SerializeField] private float buffDuration = 3f;
        [SerializeField] private float defense = 5f;

        private float? startedAt = null;

        public override void Apply(Character character)
        {
            startedAt = Time.time;
        }

        public bool Update(Character character)
        {
            if (startedAt == null)
                return false;

            if (character.IsDead)
                return true;

            if (Time.time - startedAt > duration)
            {
                return true;
            }

            List<Character> characters = AgentObject.All.OfType<Character>()
                .Where(x => character != x
                    && x.Faction == character.Faction
                    && Mathf.Abs(character.transform.position.x - x.transform.position.x) < area)
                .ToList();

            foreach (Character characterToBuff in characters)
            {
                InspiringPresenceBuff inspiringPresenceBuff = (InspiringPresenceBuff)characterToBuff.CharacterModifierHandler.Modifiers.FirstOrDefault(x => x is InspiringPresenceBuff);
                if (inspiringPresenceBuff != null)
                {
                    if (inspiringPresenceBuff.Source == this)
                    {
                        inspiringPresenceBuff.Refresh();
                    }
                }
                else
                {
                    characterToBuff.CharacterModifierHandler.Add(new InspiringPresenceBuff(character, defense, this, buffDuration));
                }
            }

            return false;
        }

        public override void OnAbilityEnded()
        {
            startedAt = null;
        }
    }
}
