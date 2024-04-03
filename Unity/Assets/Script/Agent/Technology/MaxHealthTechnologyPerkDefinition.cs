using Assets.Script.Agent.Technology;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "MaxHealth", menuName = "Definition/Technology/MaxHealth")]
    public class MaxHealthTechnologyPerkDefinition : TechnologyPerkDefinition, ITechnologyModify
    {
        public class Modifier : Modifier<Modifier>
        {
            private float maxHealth;
            public override float? MaxHealth => maxHealth;

            public Modifier(IModifiable modifiable, float maxHealth) : base(modifiable)
            {
                this.maxHealth = maxHealth;
            }
        }

        [SerializeField] private float amount;
        [SerializeField] private AgentObjectDefinition affected;

        public bool Affect(AgentObjectDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        public Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, amount);
        }
    }
}
