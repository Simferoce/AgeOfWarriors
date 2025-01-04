using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Level
    {
        public class ExecutionState : State
        {
            private class WinningCondition
            {
                public delegate void OnWinDelegate(FactionType faction);
                public event OnWinDelegate OnWin;

                private List<BaseEntity> bases;

                public void Initialize()
                {
                    bases = Entity.All.OfType<BaseEntity>().ToList();
                    foreach (BaseEntity baseEntity in bases)
                        baseEntity.EventChannelHandler.Subscribe<DeathEventChannel.Event>(OnBaseDeath);
                }

                private void OnBaseDeath(DeathEventChannel.Event evt)
                {
                    OnWin?.Invoke(evt.Entity["faction"].Get<FactionType>());
                }

                public void Dispose()
                {
                    foreach (BaseEntity baseEntity in bases)
                        baseEntity.EventChannelHandler.Unsubscribe<DeathEventChannel.Event>(OnBaseDeath);
                }
            }

            private WinningCondition winningCondition;

            public ExecutionState(Level level) : base(level)
            {
            }

            public override void Enter()
            {
                winningCondition = new WinningCondition();
                winningCondition.Initialize();

                winningCondition.OnWin += OnWin;
            }

            private void OnWin(FactionType faction)
            {
                level.stateMachine.ChangeState(new EndState(level, new BaseDestroyedEnd(faction)));
            }

            public override void Exit()
            {
                winningCondition.Dispose();
            }

            public override void Update()
            {
                level.TimeElapsed += Time.deltaTime;
            }
        }
    }
}
