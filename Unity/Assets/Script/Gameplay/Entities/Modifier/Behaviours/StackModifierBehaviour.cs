using System;

namespace Game.Modifier
{
    [Serializable]
    public class StackModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        private int currentStack = 1;

        public event Action<StackModifierBehaviour> OnStackGained;

        public int CurrentStack { get => currentStack; set => currentStack = value; }

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
    }
}
