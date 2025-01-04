using Game.UI.Windows;

namespace Game
{
    public partial class Level
    {
        public class EndState : State
        {
            private End end;

            public EndState(Level level, End end) : base(level)
            {
                this.end = end;
            }

            public override void Enter()
            {
                HudWindow hudWindow = WindowManager.Instance.GetWindow<HudWindow>();
                hudWindow.Hide();
                GameFlowManager.Instance.LoadMainMenu();
            }

            public override void Exit()
            {
            }

            public override void Update()
            {
            }
        }
    }
}
