using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using Protopia.EditorClasses.BuildUtilities.EzBuild;

namespace Protopia.EditorClasses.BuildUtilities
{
    public class EzBuildCustomBuilder
    {
        public static readonly string UserSettingsFileName = "UserSettings"; //will be a .asset file.

        /// <summary>
        /// Builds a version of the game using the user-defined build definition.
        /// </summary>
        public static void BuildEzBuildCustomDefinition()
        {
            EzBuildVarsUtility.SetBuildType(EzBuildVars.BuildTypes.Test);

            try
            {
                string folderPath = Path.Combine(GameBuilder.buildRootFolderPath, ".CustomBuild");
                var userSettings = GetUserSettings();
                string[] paths = userSettings.GetSceneListWithStandardScenes();

                bool noPreloadPaths = true;
                for (int i = 2; i < paths.Length; i++)
                {
                    if (IsStandardScenePath(paths[i])) noPreloadPaths = false;
                }

                if (noPreloadPaths)
                {
                    GameBuilder.BuildDevelopmentWindows(folderPath, true, paths);
                }
                else
                {
                    UnityEngine.Debug.LogError("Tried to make a build with duplicate preload scenes");
                }
            }
            finally
            {
                EzBuildVarsUtility.ResetBuildType();
            }
        }

        /// <summary>
        /// Builds a version of the game using only the currently active scene (and any standard scenes defined in the global settings)
        /// </summary>
        [MenuItem("Build/EzBuild Current Scene", priority = 99)]
        public static void BuildCurrentScene()
        {
            EzBuildVarsUtility.SetBuildType(EzBuildVars.BuildTypes.Test);

            try
            {
                string currentScenePath = EditorSceneManager.GetActiveScene().path;

                //make sure we're not trying to build a scene setup with multiple duplicate preload scenes.
                if (!IsStandardScenePath(currentScenePath))
                {
                    EzBuildGlobalSettings globalSettings = EzBuildGlobalSettings.GetCurrent();
                    string[] paths = new string[globalSettings.prependScenes.Count + globalSettings.appendScenes.Count + 1];
                    int index = 0;
                    for (int i = 0; i < globalSettings.prependScenes.Count; i++)
                    {
                        paths[i] = globalSettings.prependScenes[i];
                    }
                    index = globalSettings.prependScenes.Count;
                    paths[index] = currentScenePath;
                    index++;

                    for (int j = 0; j < globalSettings.appendScenes.Count; j++)
                    {
                        paths[j + index] = globalSettings.appendScenes[j];
                    }

                    string folderPath = Path.Combine(GameBuilder.buildRootFolderPath, ".CustomBuild");

                    GameBuilder.BuildDevelopmentWindows(folderPath, true, paths);
                }
                else
                {
                    UnityEngine.Debug.LogError("Tried to make a build with duplicate preload scenes");
                }
            }
            finally
            {
                EzBuildVarsUtility.ResetBuildType();
            }
        }

        private static bool IsStandardScenePath(string scenePath)
        {
            bool sceneFoundInStandardScenes = false;
            EzBuildGlobalSettings globalSettings = EzBuildGlobalSettings.GetCurrent();

            foreach (SceneReference scene in globalSettings.prependScenes)
            {
                if (scene.ScenePath == scenePath) sceneFoundInStandardScenes = true;
            }

            foreach (SceneReference scene in globalSettings.appendScenes)
            {
                if (scene.ScenePath == scenePath) sceneFoundInStandardScenes = true;
            }

            return sceneFoundInStandardScenes;
        }

        #region - Utilities -
        public static EzBuildDefinition GetUserSettings()
        {
            return (EzBuildDefinition)AssetDatabase.LoadAssetAtPath(EzBuildPaths.Folders.UserSettingsFolderPath + UserSettingsFileName + ".asset", typeof(EzBuildDefinition));

        }

        public static void EnsureSettingsAsset()
        {
            if (GetUserSettings() == null)
            {
                FolderUtils.EnsureFolderPath(EzBuildPaths.Folders.UserSettingsFolderPath);

                ScriptableObjectUtility.CreateAsset<EzBuildDefinition>(false, EzBuildPaths.Folders.UserSettingsFolderPath, UserSettingsFileName);
            }
        }

        #endregion
    }
}