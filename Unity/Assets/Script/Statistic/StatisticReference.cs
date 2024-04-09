using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public string Path { get => path; set => path = value; }

        public T GetValue(object caller)
        {
            if (string.IsNullOrEmpty(path))
                return default(T);

            string[] split = path.ToLower().Split(".");
            string current = split[0];
            string next = split.Length > 0 ? path.Substring(current.Length + 1) : string.Empty;

            if (current == "ability")
            {
                Ability ability = caller as Ability;
                return ability.Definition.GetStatistic<T>(next).GetValue(ability);
            }
            else if (current == "character")
            {
                string stat = split[1];
                if (stat == "reach")
                {
                    Character character = (caller as Ability)?.Character;

                    return (T)(object)character.Reach;
                }
                else if (stat == "attackpower")
                {
                    Character character = (caller as Ability)?.Character;

                    return (T)(object)character.AttackPower;
                }

                throw new Exception($"The stat \"{current}\" from \"{path}\" is not supported");
            }
            else
            {
                throw new Exception($"The reference \"{current}\" from \"{path}\" is not supported");
            }
        }
    }
}
