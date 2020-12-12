using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Protopia.EditorClasses.BuildUtilities;

public static class EzBuildVarsUtility
{
    public static readonly string SettingsAssetFolderPath = "Assets/Settings/BuildVars/";
    public static readonly string SettingsAssetName = "EzBuildVars.asset";

    public static string SettingsAssetPath => Path.Combine(SettingsAssetFolderPath, SettingsAssetName);

    public static EzBuildVars GetBuildVarsAsset()
    {
         return EnsureBuildVarsAsset();
    }

    private static EzBuildVars EnsureBuildVarsAsset()
    {
        var asset = AssetDatabase.LoadAssetAtPath<EzBuildVars>(SettingsAssetPath);
        if (asset == null)
        {
            FolderUtils.EnsureFolderPath(SettingsAssetFolderPath);

            ScriptableObjectUtility.CreateAsset<EzBuildVars>(false, SettingsAssetFolderPath, SettingsAssetName);
            return AssetDatabase.LoadAssetAtPath<EzBuildVars>(SettingsAssetPath);
        }
        else return asset;
    }

    public static void SetBuildType(EzBuildVars.BuildTypes buildType)
    {
        EzBuildVars asset = GetBuildVarsAsset();
        asset.SetBuildType(buildType.ToString());

        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
    }

    public static void ResetBuildType()
    {
        SetBuildType(EzBuildVars.BuildTypes.Editor);
    }

}
