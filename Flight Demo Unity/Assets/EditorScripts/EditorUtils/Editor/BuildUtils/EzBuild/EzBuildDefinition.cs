using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protopia.EditorClasses.BuildUtilities;
using UnityEditor;


namespace Protopia.EditorClasses.BuildUtilities.EzBuild
{
    /// <summary>
    /// Core of the EzBuild system. Build definitions contain simple lists of scene-references to be used in the build process.
    /// </summary>
    //[CreateAssetMenu(menuName = "SHOULD_BE_INVISIBLE/EzBuildDefinition")]
    public class EzBuildDefinition : ScriptableObject
    {
        [Header("Scenes in Build")]
        public List<SceneReference> sceneList;

        //returns the paths of every scene in this scene definition in addition to every standard scene. 
        public string[] GetSceneListWithStandardScenes()
        {
            List<string> pathList = new List<string>();

            EzBuildGlobalSettings globalSettings = EzBuildGlobalSettings.GetCurrent();

            //add prepend scenes
            foreach (SceneReference scene in globalSettings.prependScenes)
            {
                pathList.Add(scene.ScenePath);
            }

            //add scenes from this build def
            foreach (SceneReference scene in sceneList)
            {
                pathList.Add(scene.ScenePath);
            }

            //add append scenes
            foreach (SceneReference scene in globalSettings.appendScenes)
            {
                pathList.Add(scene.ScenePath);
            }

            return pathList.ToArray();
        }
    }
}