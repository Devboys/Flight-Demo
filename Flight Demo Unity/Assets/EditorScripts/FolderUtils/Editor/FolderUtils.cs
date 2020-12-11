using System.IO;
using UnityEngine;

public static class FolderUtils
{
    /// <summary>
    /// Creates an empty folder with the given path, if one does not already exist.
    /// </summary>
    public static void EnsureFolderPath(string pathRelativeToAssetsFolder)
    {
        string parsedRelativePath = pathRelativeToAssetsFolder;
        if (parsedRelativePath.StartsWith("Assets"))
        {
            parsedRelativePath = pathRelativeToAssetsFolder.Substring(7);
        }

        string fullPath = Path.Combine(Application.dataPath, parsedRelativePath);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
    }
}
