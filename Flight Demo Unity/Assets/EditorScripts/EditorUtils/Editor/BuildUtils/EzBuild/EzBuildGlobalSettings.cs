using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Protopia.EditorClasses.BuildUtilities.EzBuild
{
    //[CreateAssetMenu(menuName = "SHOULD_BE_INVISIBLE/EzBuildGlobalSettings")]
    public class EzBuildGlobalSettings : ScriptableObject
    {
        [SerializeField] private SceneReferenceSO currentMainScene = null;
        [Label("These scenes are added before any build def scenes in BuildSettings when building using EzBuild.")]
        [Space]
        [Space]
        public List<SceneReference> prependScenes;
        [Label("These scenes are added after any build def scenes in BuildSettings when building using EzBuild.")]
        [Space]
        [Space]
        public List<SceneReference> appendScenes;

        /// <summary>
        /// REWORK THIS, PLEASE
        /// </summary>
        public void SetMainScene(string path)
        {
            currentMainScene.buildMainScenePath = path;

            //ensure asset is saved before build
            EditorUtility.SetDirty(currentMainScene);
            AssetDatabase.SaveAssets();
        }
        public void ResetMainScene()
        {
            currentMainScene.ResetPath();

            //ensure asset is saved before build
            EditorUtility.SetDirty(currentMainScene);
            AssetDatabase.SaveAssets();
        }

        public static EzBuildGlobalSettings GetCurrent()
        {
            EzBuildGlobalSettings globalSettings = AssetDatabase.LoadAssetAtPath<EzBuildGlobalSettings>(EzBuildPaths.Assets.GlobalSettingsAssetPath);
            return globalSettings;
        }
    }
}
