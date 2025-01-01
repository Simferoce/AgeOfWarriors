using System.Collections;

namespace Game
{
    public class Level : IGameState
    {
        private LevelDefinition levelDefinition;

        public Level(LevelDefinition levelDefinition)
        {
            this.levelDefinition = levelDefinition;
        }

        public void Load()
        {
            GameSceneManager.Instance.StartCoroutine(Operation());

            IEnumerator Operation()
            {
                yield return GameSceneManager.Instance.Load(levelDefinition.SceneDefinition);
            }
        }

        public void Update()
        {
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}