using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game
{
    public class BoostrapReadyNotification : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            Bootstrap.Initialize();
        }
    }

    [InitializeOnLoad]
    public class Bootstrap
    {
        public abstract class Launcher
        {
            protected Dictionary<string, object> fields = new Dictionary<string, object>();

            public event Action OnModified;

            public abstract void Launch();
            public abstract void Load();

            protected T Load<T>(string key)
                where T : UnityEngine.Object
            {
                if (EditorPrefs.HasKey(key))
                {
                    string levelDefinitionPath = EditorPrefs.GetString(key);

                    if (!string.IsNullOrEmpty(levelDefinitionPath))
                        return AssetDatabase.LoadAssetAtPath<T>(levelDefinitionPath);
                }

                return null;
            }

            public virtual void Set<T>(string key, T data)
                where T : UnityEngine.Object
            {
                EditorPrefs.SetString(key, data != null ? AssetDatabase.GetAssetPath(data) : string.Empty);
                fields[key] = data;
                OnModified?.Invoke();
            }

            public T Get<T>(string key)
            {
                return (T)fields[key];
            }

            public abstract string GetDescription();
        }

        public class LevelLauncher : Launcher
        {
            public const string LevelDefinitionKey = "SceneLauncher_LevelDefinition";
            public const string CommanderDefinitionKey = "SceneLauncher_PlayerCommanderDefinition";

            public LevelLauncher()
            {
                fields.Add(LevelDefinitionKey, null);
                fields.Add(CommanderDefinitionKey, null);
            }

            public override void Load()
            {
                fields[LevelDefinitionKey] = Load<LevelDefinition>(LevelDefinitionKey);
                fields[CommanderDefinitionKey] = Load<CommanderDefinition>(CommanderDefinitionKey);
            }

            public override void Launch()
            {
                GameFlowManager.Instance.LoadLevel((LevelDefinition)fields[LevelDefinitionKey], new Agent.AgentLoadout() { CommanderDefinition = (CommanderDefinition)fields[CommanderDefinitionKey] });
            }

            public override string GetDescription()
            {
                return $"Level - {(fields[LevelDefinitionKey] != null ? (fields[LevelDefinitionKey] as LevelDefinition).Title : "Not Specified")}";
            }
        }

        public class MainMenuLauncher : Launcher
        {
            public override string GetDescription()
            {
                return "MainMenu";
            }

            public override void Launch()
            {
                GameFlowManager.Instance.LoadMainMenu();
            }

            public override void Load()
            {
            }
        }

        public static Launcher CurrentLauncher { get; private set; }
        public static event System.Action OnModified;

        private static bool HasBeenInitialized = false;

        public static void Initialize()
        {
            if (HasBeenInitialized)
                return;

            HasBeenInitialized = true;

            string[] guids = AssetDatabase.FindAssets("t:scene 00-Empty");
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;

            if (EditorPrefs.HasKey("SceneLancher_LauncherType"))
            {
                ChangeLauncher(Type.GetType(EditorPrefs.GetString("SceneLancher_LauncherType")));
            }
            else
            {
                ChangeLauncher(typeof(MainMenuLauncher));
            }
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                string[] guids = AssetDatabase.FindAssets("t:prefab Universe");
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

                GameObject universePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Managers/Universe.prefab");
                GameObject universeGameObject = GameObject.Instantiate(universePrefab);
                Universe universe = universeGameObject.GetComponent<Universe>();
                universe.OnLoaded += Universe_OnLoaded;
            }
        }

        private static void Universe_OnLoaded(Universe universe)
        {
            universe.OnLoaded -= Universe_OnLoaded;
            CurrentLauncher.Launch();
        }

        public static void ChangeLauncher(Type type)
        {
            if (CurrentLauncher != null)
                CurrentLauncher.OnModified -= CurrentLauncher_OnModified;

            CurrentLauncher = (Launcher)Activator.CreateInstance(type);
            CurrentLauncher.OnModified += CurrentLauncher_OnModified;

            CurrentLauncher.Load();
            EditorPrefs.SetString("SceneLancher_LauncherType", type.FullName);
            OnModified?.Invoke();
        }

        private static void CurrentLauncher_OnModified()
        {
            OnModified?.Invoke();
        }
    }
}