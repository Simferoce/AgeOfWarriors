using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Game.Bootstrap;
using Object = UnityEngine.Object;

namespace Game
{
    public class SceneLauncher : EditorWindow
    {
        public abstract class LauncherDrawer
        {
            public abstract void Initialize(VisualElement root);

            public abstract Type GetLauncherType();
        }

        public abstract class LauncherDrawer<T> : LauncherDrawer
            where T : Launcher
        {
            protected T launcher;

            protected LauncherDrawer(T launcher)
            {
                this.launcher = launcher;
            }

            public override Type GetLauncherType()
            {
                return typeof(T);
            }
        }

        public class MainMenuLauncherDrawer : LauncherDrawer<MainMenuLauncher>
        {
            public MainMenuLauncherDrawer(MainMenuLauncher launcher) : base(launcher)
            {
            }

            public override void Initialize(VisualElement root)
            {
            }
        }

        public class LevelLauncherDrawer : LauncherDrawer<LevelLauncher>
        {
            private ObjectField levelField;
            private ObjectField commanderField;

            public LevelLauncherDrawer(LevelLauncher launcher) : base(launcher)
            {
            }

            public override void Initialize(VisualElement root)
            {
                levelField = CreatePreferenceObjectField<LevelDefinition>("Level", "SceneLauncher_LevelDefinition");
                commanderField = CreatePreferenceObjectField<CommanderDefinition>("Player Commander", "SceneLauncher_PlayerCommanderDefinition");

                root.Add(levelField);
                root.Add(commanderField);
            }

            private ObjectField CreatePreferenceObjectField<T>(string label, string key)
                where T : Object
            {
                ObjectField objectField = new ObjectField(label);
                objectField.objectType = typeof(T);

                objectField.value = launcher.Get<T>(key);
                objectField.RegisterValueChangedCallback(OnLevelDefinitionChange);

                void OnLevelDefinitionChange(ChangeEvent<Object> evt)
                {
                    launcher.Set<T>(key, (T)evt.newValue);
                }

                return objectField;
            }
        }

        private List<Type> options = new List<Type>() { typeof(MainMenuLauncher), typeof(LevelLauncher) };
        private Dictionary<Type, Type> launcherDrawers = new Dictionary<Type, Type>()
        {
            {typeof(MainMenuLauncher), typeof(MainMenuLauncherDrawer) },
            {typeof(LevelLauncher), typeof(LevelLauncherDrawer) },
        };

        private LauncherDrawer drawer = null;
        private DropdownField launcherChoiceField;
        private VisualElement launcherRoot;

        [MenuItem("Tools/SceneLauncher")]
        public static void ShowSceneLauncher()
        {
            SceneLauncher wnd = GetWindow<SceneLauncher>();
            wnd.titleContent = new GUIContent("SceneLauncher");
        }

        public void CreateGUI()
        {
            launcherChoiceField = new DropdownField(options.Select(x => x.ToString()).ToList(), options.IndexOf(Bootstrap.CurrentLauncher.GetType()));
            launcherChoiceField.RegisterValueChangedCallback(OnLauncherOptionChange);
            rootVisualElement.Add(launcherChoiceField);

            launcherRoot = new VisualElement();
            rootVisualElement.Add(launcherRoot);

            RefreshLauncher();

            void OnLauncherOptionChange(ChangeEvent<string> evt)
            {
                Type launcherType = options[launcherChoiceField.index];
                Bootstrap.ChangeLauncher(launcherType);
                RefreshLauncher();
            }
        }

        private void RefreshLauncher()
        {
            if (drawer == null || Bootstrap.CurrentLauncher.GetType() != drawer.GetLauncherType())
            {
                launcherRoot.Clear();

                LauncherDrawer drawer = (LauncherDrawer)Activator.CreateInstance(launcherDrawers[Bootstrap.CurrentLauncher.GetType()], new object[] { Bootstrap.CurrentLauncher });
                this.drawer = drawer;

                drawer.Initialize(launcherRoot);
            }
        }
    }
}
