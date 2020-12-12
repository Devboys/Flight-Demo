using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Protopia.EditorClasses.BuildUtilities.EzBuild;
using System;

namespace Protopia.EditorClasses.BuildUtilities
{
    public static class MainDefBuilder
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
            EzBuildVarsUtility.SetBuildType(EzBuildVars.BuildTypes.Main);

            try
            {
                //extract scene paths from main-build definition.
                var buildDef = AssetDatabase.LoadAssetAtPath<EzBuildDefinition>(EzBuildPaths.Folders.BuildDefinitionsRootFolderPath + "MainBuild.asset");
                string[] paths = buildDef.GetSceneListWithStandardScenes();

                //construct output folder path relative to build root folder.
                string folderPath = Path.Combine(ArchivedBuildFolderName, archivedBuildSubfolderName);

                //Make the build.
                GameBuilder.BuildWindows(folderPath, false, paths);
            }
            finally
            {
                //Reset the build type back to default.
                EzBuildVarsUtility.ResetBuildType();
            }
        }

        [MenuItem("Build/Main/QuickBuild: Windows", false, 1)]
        public static void QuickBuildWindows()
        {
            //Set build type for any debug info.
            EzBuildVarsUtility.SetBuildType(EzBuildVars.BuildTypes.Main);
            try
            {
                //extract scene paths from main-build definition.
                var buildDef = AssetDatabase.LoadAssetAtPath<EzBuildDefinition>(EzBuildPaths.Folders.BuildDefinitionsRootFolderPath + "MainBuild.asset");
                string[] paths = buildDef.GetSceneListWithStandardScenes();

                //construct output folder path relative to build root folder.
                string folderPath = Path.Combine(GameBuilder.buildRootFolderPath, QuickBuildFolderName);

                //Make the build.
                GameBuilder.BuildWindows(folderPath, true, paths);
            }
            finally
            {
                //Reset the build type back to default.
                EzBuildVarsUtility.ResetBuildType();
            }
        }

        [MenuItem("Build/Main/DevBuild: Windows", false, 3)]
        public static void DevBuildWindows()
        {
            //Set build type for any debug info.
            EzBuildVarsUtility.SetBuildType(EzBuildVars.BuildTypes.Main);

            try
            {
                //extract scene paths from main-build definition.
                var buildDef = AssetDatabase.LoadAssetAtPath<EzBuildDefinition>(EzBuildPaths.Folders.BuildDefinitionsRootFolderPath + "MainBuild.asset");
                string[] paths = buildDef.GetSceneListWithStandardScenes();

                //construct output folder path relative to build root folder.
                string folderPath = Path.Combine(GameBuilder.buildRootFolderPath, DevBuildFolderName);

                //Make the build
                GameBuilder.BuildDevelopmentWindows(folderPath, true, paths);
            }
            finally
            {
                //Reset the build type back to default.
                EzBuildVarsUtility.ResetBuildType();
            }
        }

    }
}
