using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildVersionHelperWindow : EditorWindow
{
    private GUIStyle descriptionStyle;
    private string description = "This is a small helper tool to change the current build version of the game that helps with following game build naming convention.";

    private enum DevelopmentPhase
    {
        Demo,
        Alpha,
        Beta,
        RC,
        Release
    }

    private static GUIContent releaseVersionContent = new GUIContent("Release Version", "Release Version: 0 when game is in development, 1 when its released.");
    private int releaseVersion = 0;

    private static GUIContent majorVersionContent = new GUIContent("Major Version", "Major version: Currently incremented weekly, indicates milestone builds.");
    private int majorVersion = 0;

    private static GUIContent minorVersionContent = new GUIContent("Minor Version", "Minor Version: Implemented whenever a feature is functionally added to the game.");
    private int minorVersion = 0;

    private static GUIContent stabilityVersionContent = new GUIContent("Stability Version", "Stability Version: Indicates bug stability. Starts at A and is alphabetically incremented whenever a new 'fixed' build gets sent to QA's for regression testing.");
    private char stabilityVersion = 'A';

    private static GUIContent devPhaseContent = new GUIContent("Development Phase", "Development Phase: Should match the current development phase we're in.");
    private DevelopmentPhase devPhase = DevelopmentPhase.Demo;

    private string FullVersion => $"{releaseVersion}.{majorVersion}.{minorVersion}{stabilityVersion}_{devPhase}";

    [MenuItem("Pipeline/Build Version Helper")]
    static void Init()
    {
        BuildVersionHelperWindow window = (BuildVersionHelperWindow) GetWindowWithRect(
            t: typeof(BuildVersionHelperWindow), 
            rect: new Rect(0, 0, 300, 220), 
            utility: true, 
            title: "Build Version Helper");

        window.CreateGUIOptions();
        window.SetDefaultValuesFromCurrentVersion();
    }


    private void OnGUI()
    {
        EditorGUILayout.LabelField(description, descriptionStyle);

        releaseVersion = EditorGUILayout.IntField(releaseVersionContent, releaseVersion);
        majorVersion = EditorGUILayout.IntField(majorVersionContent, majorVersion);
        minorVersion = EditorGUILayout.IntField(minorVersionContent, minorVersion);
        stabilityVersion = EditorGUILayout.TextField(stabilityVersionContent, stabilityVersion.ToString()).ToCharArray()[0];
        devPhase = (DevelopmentPhase) EditorGUILayout.EnumPopup(devPhaseContent, devPhase);

        EditorGUILayout.LabelField("Version Preview", FullVersion);
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Current Version", Application.version, EditorStyles.boldLabel);

        if (GUILayout.Button("Commit version"))
        {
            CommitVersion();
        }

        Validate();
    }

    private void Validate()
    {
        releaseVersion = Mathf.Clamp(releaseVersion, 0, 9999);
        majorVersion = Mathf.Clamp(majorVersion, 0, 9999);
        minorVersion = Mathf.Clamp(minorVersion, 1, 9999);
        stabilityVersion = char.ToUpper(stabilityVersion);
    }

    private void CommitVersion()
    {
        PlayerSettings.bundleVersion = FullVersion;
    }

    public void CreateGUIOptions()
    {
        descriptionStyle = new GUIStyle(EditorStyles.helpBox) { alignment = TextAnchor.MiddleCenter };
    }

    public void SetDefaultValuesFromCurrentVersion()
    {
        try
        {
            //parse current application version
            string applicationVersion = Application.version.ToString();
            string[] splitString = applicationVersion.Split(".".ToCharArray()[0]);
            string release = splitString[0].ToString();
            string major = splitString[1].ToString(); ;
            string minorUnsplit = splitString[2].ToString();
            string minor = minorUnsplit.ToCharArray()[0].ToString();
            char stability = minorUnsplit.ToCharArray()[1];
            string phase = minorUnsplit.Split("_".ToCharArray()[0])[1];

            releaseVersion = int.Parse(release);
            majorVersion = int.Parse(major);
            minorVersion = int.Parse(minor);
            stabilityVersion = stability;
            devPhase = (DevelopmentPhase) Enum.Parse(typeof(DevelopmentPhase), phase);
        }
        catch(Exception)
        {
            Debug.LogError("Parsing current version failed in Build Version Helper. Using default values.");
        }
    }

}
