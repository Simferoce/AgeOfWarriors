namespace Game
{
    public class AbilityCastingContextElement : ContextElement
    {
        public Ability Ability { get; set; }

        public AbilityCastingContextElement(Ability ability)
        {
            Ability = ability;
        }
    }
}
