using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SceneDefinition", menuName = "Definition/SceneDefinition")]
    public class SceneDefinition : Definition
    {
        [SerializeField] private AssetReferenceScene scene;

        public AssetReferenceScene Scene { get => scene; }
    }
}
