using AgeOfWarriors;
using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class Definition : ScriptableObject, IDefinition
    {
        [SerializeField] private string id;

        public string Id { get => id; set => id = value; }
    }
}