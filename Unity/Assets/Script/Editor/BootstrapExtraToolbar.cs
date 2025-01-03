using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    [InitializeOnLoad]
    public class BootstrapExtraToolbar
    {
        private static Label label;
        private static bool initialized = false;

        static BootstrapExtraToolbar()
        {
            Bootstrap.OnModified += Bootstrap_OnModified;
            ExtraToolbar.OnToolbarGUIRight = DrawCurrent;
        }

        private static void Bootstrap_OnModified()
        {
            if (!initialized)
                return;

            label.text = Bootstrap.CurrentLauncher.GetDescription();
        }

        private static void DrawCurrent(VisualElement visualElement)
        {
            initialized = true;

            label = new Label(Bootstrap.CurrentLauncher.GetDescription());
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.width = 100;

            Image button = new Image();
            button.image = EditorGUIUtility.IconContent("d_ScaleTool On").image;
            button.style.width = 16;
            button.style.height = 16;
            button.style.marginLeft = 8;
            button.style.borderBottomRightRadius = new StyleLength(7);
            button.style.borderBottomLeftRadius = new StyleLength(7);
            button.style.borderTopLeftRadius = new StyleLength(7);
            button.style.borderTopRightRadius = new StyleLength(7);

            button.RegisterCallback<ClickEvent>(evt =>
            {
                SceneLauncher.ShowSceneLauncher();
            });

            button.RegisterCallback<MouseEnterEvent>(evt =>
            {
                button.style.backgroundColor = new StyleColor(new Color(0.3f, 0.3f, 0.3f)); // Example hover color
            });

            button.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                button.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f)); // Reset background color
            });

            visualElement.style.flexDirection = FlexDirection.Row;
            visualElement.style.width = new StyleLength(Length.Auto());
            visualElement.style.height = new StyleLength(Length.Auto());
            visualElement.style.paddingLeft = 5;
            visualElement.style.paddingRight = 5;
            visualElement.style.paddingTop = 2;
            visualElement.style.paddingBottom = 2;
            visualElement.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
            visualElement.style.flexGrow = 0;
            visualElement.style.borderBottomRightRadius = new StyleLength(6);
            visualElement.style.borderBottomLeftRadius = new StyleLength(3);
            visualElement.style.borderTopLeftRadius = new StyleLength(3);
            visualElement.style.borderTopRightRadius = new StyleLength(6);

            visualElement.Add(label);
            visualElement.Add(button);
        }

    }
}
