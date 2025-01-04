using Game.Character;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CharacterInspectorWindowOpener : MonoBehaviour
    {
        [SerializeField] private CharacterEntity character;

        private void Awake()
        {
            GetComponentInParent<Canvas>().worldCamera = UnityEngine.Camera.main;
        }

        public void OpenDetail()
        {
            CharacterInspectorWindow characterInspectorWindow = WindowManager.Instance.GetWindow<CharacterInspectorWindow>();
            characterInspectorWindow.Show(character);
        }
    }
}
