using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Protopia.EditorClasses.BuildUtilities.EzBuild;
using UnityEngine.UIElements;

/// <summary>
/// This class is responsible for adding the EzBuild settings option to the "Project Settings" window.
/// </summary>
public class EzBuildSettingsProvider : SettingsProvider
{
    /// <summary>
    /// This is our reference to the serialized object
    /// </summary>
    private SerializedObject m_EzBuildSettings;

    public EzBuildSettingsProvider(string path, SettingsScope scope) : base(path, scope)
    { }

    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        m_EzBuildSettings = EzBuildGlobalSettings.GetSerializedSettings();
    }

    public override void OnGUI(string searchContext)
    {
        m_EzBuildSettings.Update();

        EditorGUILayout.PropertyField(m_EzBuildSettings.FindProperty("prependScenes"));
        EditorGUILayout.PropertyField(m_EzBuildSettings.FindProperty("appendScenes"));

        m_EzBuildSettings.ApplyModifiedProperties();
    }

    public static void EnsureSettingsAvailable()
    {
        EzBuildGlobalSettings.GetOrCreate();
    }

    //register the settings provider
    [SettingsProvider]
    public static SettingsProvider CreateEzBuildSettingsProvider()
    {
        EnsureSettingsAvailable();

        var provider = new EzBuildSettingsProvider("Project/EzBuild Settings", SettingsScope.Project);
        //provider.keywords = 
        return provider;
    }
}
