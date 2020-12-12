using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Protopia.EditorClasses.BuildUtilities.EzBuild
{
    //[CreateAssetMenu(menuName = "SHOULD_BE_INVISIBLE/EzBuildGlobalSettings")]
    public class EzBuildGlobalSettings : ScriptableObject
    {
        [Label("These scenes are added before and after any build def scenes in BuildSettings when building using EzBuild.")]
        [Header("Standard Scenes")]
        public List<SceneReference> prependScenes;
        public List<SceneReference> appendScenes;

        /// <summary>
        /// Returns a ref to the current asset.
        /// </summary>
        public static EzBuildGlobalSettings GetCurrent()
        {
            EzBuildGlobalSettings globalSettings = AssetDatabase.LoadAssetAtPath<EzBuildGlobalSettings>(EzBuildPaths.Assets.GlobalSettingsAssetPath);
            return globalSettings;
        }
    }
}
