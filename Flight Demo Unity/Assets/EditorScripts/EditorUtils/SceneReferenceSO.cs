using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SHOULD_BE_INVISIBLE/SceneReferenceSO")]
public class SceneReferenceSO : ScriptableObject
{
    public SceneReference defaultScene;

    public string buildMainScenePath;

    public string GetScene()
    {
        if (Application.isEditor || buildMainScenePath == "") return defaultScene.ScenePath;

        else return buildMainScenePath;
    }

    public void ResetPath()
    {
        buildMainScenePath = defaultScene.ScenePath;
    }
}
