using System.Text;
using UnityEditor;
using UnityEngine;

public class ClearMissingTypeFromGameObject
{
    [MenuItem("Tools/Clear References with Missing Types")]
    static public void ClearMissingReference()
    {
        var report = new StringBuilder();

        var guids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets" });
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (obj != null)
            {
                bool hasMissing = false;
                foreach (MonoBehaviour item in obj.GetComponentsInChildren<MonoBehaviour>())
                {
                    bool result = SerializationUtility.HasManagedReferencesWithMissingTypes(item);

                    if (SerializationUtility.ClearAllManagedReferencesWithMissingTypes(item))
                    {
                        hasMissing = true;
                    }
                }

                if (hasMissing)
                {
                    report.Append("Cleared missing types from ").Append(path).AppendLine();
                }
                else
                {
                    report.Append("No missing types to clear on ").Append(path).AppendLine();
                }
            }
        }

        Debug.Log(report.ToString());
    }
}