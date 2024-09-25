using System;

namespace Game
{
    [Serializable]
    public class StackModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        private int currentStack;

        public event Action<StackModifierBehaviour> OnStackGained;

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
