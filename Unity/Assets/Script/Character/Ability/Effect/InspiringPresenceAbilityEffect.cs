using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InspiringPresenceAbilityEffect : AbilityEffect, ILingeringAbilityEffect
    {
        private class InspiringPresenceBuff : Modifier<InspiringPresenceBuff>
        {
            public InspiringPresenceAbilityEffect Source { get; set; }
            public override float? Defense => defense;

            private float defense;

            public InspiringPresenceBuff(Character character, float defense, InspiringPresenceAbilityEffect source)
                : base(character)
            {
                this.defense = defense;
                this.Source = source;
            }
        }

        [SerializeField] private float duration = 5f;
        [SerializeField] private float area = 2f;
        [SerializeField] private float buffDuration = 3f;
        [SerializeField] private float defense = 5f;

        private float? startedAt = null;

        public override void Apply()
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
                InspiringPresenceBuff inspiringPresenceBuff = (InspiringPresenceBuff)characterToBuff.ModifierHandler.Modifiers.FirstOrDefault(x => x is InspiringPresenceBuff);
                if (inspiringPresenceBuff != null)
                {
                    if (inspiringPresenceBuff.Source == this)
                    {
                        inspiringPresenceBuff.Refresh();
                    }
                }
                else
                {
                    inspiringPresenceBuff = new InspiringPresenceBuff(character, defense, this)
                        .With(new CharacterModifierTimeElement(buffDuration));

                    characterToBuff.ModifierHandler.Add(inspiringPresenceBuff);
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
