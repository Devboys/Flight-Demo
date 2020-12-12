using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Protopia.EditorClasses.BuildUtilities.EzBuild
{
    public class EzBuildGlobalSettings : ScriptableObject
    {
        //Asset path
        public static readonly string folderPath = "Assets/Settings/EzBuild/";
        public static readonly string fileName = "EzBuildGlobalSettings.asset";
        public static string AssetFullPath => Path.Combine(folderPath, fileName);

        [Label("These scenes are added before and after any build def scenes in BuildSettings when building using EzBuild.")]
        [Header("Standard Scenes")]
        public List<SceneReference> prependScenes;
        public List<SceneReference> appendScenes;

        /// <summary>
        /// Returns a reference to the current asset. Will return null if no such asset exists.
        /// </summary>
        public static EzBuildGlobalSettings GetCurrent()
        {
            EzBuildGlobalSettings globalSettings = AssetDatabase.LoadAssetAtPath<EzBuildGlobalSettings>(AssetFullPath);
            return globalSettings;
        }

        /// <summary>
        /// Gets a reference to the current asset. Creates one if it does not exist.
        /// </summary>
        /// <returns></returns>
        public static EzBuildGlobalSettings GetOrCreate()
        {
            var settings = GetCurrent();

            if(settings == null)
            {
                settings = ScriptableObjectUtility.CreateAsset<EzBuildGlobalSettings>(false, folderPath, fileName, false);
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreate());
        }

    }
}
