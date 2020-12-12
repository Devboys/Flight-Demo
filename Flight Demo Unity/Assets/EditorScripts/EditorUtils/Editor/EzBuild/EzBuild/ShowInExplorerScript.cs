using System.IO;
using UnityEditor;

namespace Protopia.EditorClasses.BuildUtilities.EzBuild
{
    public static class ShowInExplorerScript
    {
        public static readonly string RootFolderDescriptorFileName = "This Folder is for EzBuild Builds";

        public static string DescriptorPath => Path.Combine(GameBuilder.buildRootFolderPath, RootFolderDescriptorFileName);

        [MenuItem("Build/Open Build Folder", priority = 101)]
        public static void OpenInExplorer()
        {
            EnsureDescriptorFile();

            EditorUtility.RevealInFinder(DescriptorPath);
        }

        private static void EnsureDescriptorFile()
        {
            FolderUtils.EnsureFolderPath(GameBuilder.buildRootFolderPath);

            if (!File.Exists(DescriptorPath))
            {
                File.Create(DescriptorPath);
            }
        }
    }
}