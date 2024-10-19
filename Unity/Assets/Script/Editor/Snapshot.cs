using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class Snapshot
    {
        private static bool isSnapshotting = false;
        private static string path;
        private static UnityEngine.Camera camera;
        private static RenderTexture renderTexture;
        private static float startedAt;

        [MenuItem("CONTEXT/Camera/Snapshot")]
        public static void SnapshotCommand(MenuCommand command)
        {
            if (isSnapshotting)
                return;

            camera = command.context as UnityEngine.Camera;
            path = PlayerPrefs.GetString("snapshot_file_path", "");
            path = EditorUtility.SaveFilePanel("Snapshot", path, "snapshot", "png");

            if (!string.IsNullOrEmpty(path))
            {
                startedAt = Time.realtimeSinceStartup;

                isSnapshotting = true;
                renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
                renderTexture.Create();

                camera.targetTexture = renderTexture;

                EditorApplication.update += MakeSnapshot;
            }
        }

        private static void MakeSnapshot()
        {
            if (Time.realtimeSinceStartup - startedAt > 1f)
            {
                EditorApplication.update -= MakeSnapshot;

                PlayerPrefs.SetString("snapshot_file_path", path);
                Texture2D texture2D = ToTexture2D(camera.targetTexture);
                byte[] bytes = texture2D.EncodeToPNG();

                File.WriteAllBytes(path, bytes);
                Object.DestroyImmediate(texture2D);
                camera.targetTexture = null;
                renderTexture.Release();
                Object.DestroyImmediate(renderTexture);

                isSnapshotting = false;
            }
        }

        private static Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(512, 512, TextureFormat.ARGB32, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}