using Game;

namespace Assets.Script.Agent.Technology
{
    public interface ITechnologyModify
    {
        public bool Affect(AgentObjectDefinition definition);
        public Modifier GetModifier(ModifierHandler modifiable);
    }
}
