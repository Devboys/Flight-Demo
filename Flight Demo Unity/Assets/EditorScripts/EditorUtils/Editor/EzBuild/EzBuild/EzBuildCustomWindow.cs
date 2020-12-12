using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Protopia.EditorClasses.BuildUtilities;

namespace Protopia.EditorClasses.BuildUtilities.EzBuild {
    public class EzBuildCustomWindow : EditorWindow
    {

        [MenuItem("Build/EzBuild Custom", priority = 100)]
        public static void Init()
        {
            EzBuildCustomWindow window = (EzBuildCustomWindow)GetWindow(typeof(EzBuildCustomWindow), false, "Ez Build Setup");
        }

        private void OnGUI()
        {
            EzBuildCustomBuilder.EnsureSettingsAsset();

            //get the current user settings;
            EzBuildDefinition userSettings = EzBuildCustomBuilder.GetUserSettings();

            if (userSettings != null)
            {
                Editor ed = Editor.CreateEditor(userSettings);
                ed.OnInspectorGUI();

                GUILayout.Space(10);

                if (GUILayout.Button("Custom Build"))
                {
                    EzBuildCustomBuilder.BuildEzBuildCustomDefinition();
                }
            }
        }

    }
}
