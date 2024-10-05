using System;

namespace Game
{
    [Serializable]
    public class ModifierBehaviourStack : ModifierBehaviour, IModifierStack
    {
        private int currentStack;

        public event Action<ModifierBehaviourStack> OnStackGained;

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
