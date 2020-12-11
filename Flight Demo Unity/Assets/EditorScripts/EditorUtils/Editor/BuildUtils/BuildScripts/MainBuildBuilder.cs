using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Protopia.EditorClasses.BuildUtilities.EzBuild;
using System;

namespace Protopia.EditorClasses.BuildUtilities
{
    public static class MainBuildBuilder
    {

        public static readonly string QuickBuildFolderName = ".QuickBuild";
        public static readonly string ArchivedBuildFolderName = "ArchivedBuilds";
        public static readonly string DevBuildFolderName = ".DevBuild";
        public static string archivedBuildSubfolderName
        {
            get
            {
                return $"{Application.productName}_v{Application.version}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}";
            }
        }

        [MenuItem("Build/Main/Archived Build: Windows", false, 2)]
        public static void ArchivedBuildWindows()
        {
            //Set build type for any debug info.
            EditorBuildInfoVarsUtility.SetBuildType(EditorBuildInfoVarsUtility.BuildTypes.Main);
            try
            {
                //extract scene paths from main-build definition.
                var buildDef = AssetDatabase.LoadAssetAtPath<EzBuildDefinition>(EzBuildPaths.Folders.BuildDefinitionsRootFolderPath + "MainBuild.asset");
                string[] paths = buildDef.GetSceneListWithStandardScenes();

                //construct output folder path relative to build root folder.
                string folderPath = Path.Combine(ArchivedBuildFolderName, archivedBuildSubfolderName);

                //Make & package the build.
                GameBuilder.BuildWindows(folderPath, false, paths);

                //Reset current scene ref (pretty jank).
                EzBuildGlobalSettings.GetCurrent().ResetMainScene();
            }
            finally
            {
                //Set build type asset.
                EditorBuildInfoVarsUtility.SetBuildType(EditorBuildInfoVarsUtility.BuildTypes.Editor);
            }
        }

        [MenuItem("Build/Main/QuickBuild: Windows", false, 1)]
        public static void QuickBuildWindows()
        {
            EditorBuildInfoVarsUtility.SetBuildType(EditorBuildInfoVarsUtility.BuildTypes.Main);

            var buildDef = AssetDatabase.LoadAssetAtPath<EzBuildDefinition>(EzBuildPaths.Folders.BuildDefinitionsRootFolderPath + "MainBuild.asset");
            string folderPath = Path.Combine(GameBuilder.buildRootFolderPath, QuickBuildFolderName);
            string[] paths = buildDef.GetSceneListWithStandardScenes();
            GameBuilder.BuildWindows(folderPath, true, paths);

            EzBuildGlobalSettings.GetCurrent().ResetMainScene();
            EditorBuildInfoVarsUtility.SetBuildType(EditorBuildInfoVarsUtility.BuildTypes.Editor);
        }

        [MenuItem("Build/Main/DevBuild: Windows", false, 3)]
        public static void DevBuildWindows()
        {
            EditorBuildInfoVarsUtility.SetBuildType(EditorBuildInfoVarsUtility.BuildTypes.Main);

            var buildDef = AssetDatabase.LoadAssetAtPath<EzBuildDefinition>(EzBuildPaths.Folders.BuildDefinitionsRootFolderPath + "MainBuild.asset");
            string folderPath = Path.Combine(GameBuilder.buildRootFolderPath, DevBuildFolderName);
            string[] paths = buildDef.GetSceneListWithStandardScenes();
            GameBuilder.BuildDevelopmentWindows(folderPath, true, paths);

            EzBuildGlobalSettings.GetCurrent().ResetMainScene();
            EditorBuildInfoVarsUtility.SetBuildType(EditorBuildInfoVarsUtility.BuildTypes.Editor);
        }
    }
}
