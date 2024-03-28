using UnityEditor;

public class DefinitionImport : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        foreach (string str in importedAssets)
        {
            Game.Definition definition = AssetDatabase.LoadAssetAtPath<Game.Definition>(str);
            if (definition != null && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(definition, out string guid, out long _))
            {
                definition.Id = guid;
            }
        }
    }
}
