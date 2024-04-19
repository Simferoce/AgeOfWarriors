namespace Game
{
    public class StackModifierElement : ModifierElement
    {
        private int currentStack;

        public event System.Action<StackModifierElement> OnStackGained;

        public int CurrentStack { get => currentStack; set => currentStack = value; }

        public override void Initialize()
        {
        }

        public override void Refresh()
        {
            IncreaseStack();
        }

        public void IncreaseStack()
        {
            currentStack++;
            OnStackGained?.Invoke(this);
        }

        public void DecreaseStack()
        {
            currentStack--;
        }

        public override bool Update()
        {
            return false;
        }
    }
}
