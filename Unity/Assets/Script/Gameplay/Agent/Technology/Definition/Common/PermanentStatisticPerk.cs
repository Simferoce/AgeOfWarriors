namespace Game
{
    public class PermanentStatisticPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
