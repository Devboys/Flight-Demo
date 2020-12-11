using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Protopia.EditorClasses.BuildUtilities;

public static class EditorBuildInfoVarsUtility
{
    public static readonly string SettingsAssetFolderPath = "Assets/Settings/BuildInfo/";
    public static readonly string SettingsAssetName = "BuildInfoVars.asset";

    public static string SettingsAssetPath => Path.Combine(SettingsAssetFolderPath, SettingsAssetName);

    public enum BuildTypes
    {
        Editor,
        Test,
        Main
    }

    public static BuildInfoVars GetInfoVars()
    {
         return EnsureInfoVarsAsset();
    }

    private static BuildInfoVars EnsureInfoVarsAsset()
    {
        var asset = AssetDatabase.LoadAssetAtPath<BuildInfoVars>(SettingsAssetPath);
        if (asset == null)
        {
            FolderUtils.EnsureFolderPath(SettingsAssetFolderPath);

            ScriptableObjectUtility.CreateAsset<BuildInfoVars>(false, SettingsAssetFolderPath, SettingsAssetName);
            return AssetDatabase.LoadAssetAtPath<BuildInfoVars>(SettingsAssetPath);
        }
        else return asset;
    }

    public static void SetBuildType(BuildTypes buildType)
    {
        BuildInfoVars asset = GetInfoVars();
        asset.SetBuildType(buildType.ToString());
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
    }

}
