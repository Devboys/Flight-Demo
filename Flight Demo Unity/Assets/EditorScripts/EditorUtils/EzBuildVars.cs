using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a scriptable object that contains build variables which can be set from the editor. Use EzBuildVarUtility to get a reference to it.
/// </summary>
public class EzBuildVars : ScriptableObject
{
    /// <summary>
    /// Types of builds that can be made using EzBuild. Can be expanded if necessary.
    /// </summary>
    public enum BuildTypes
    {
        Editor, //Build type if playing from the editor.
        Test,   //Build type if building using a non-main build def.
        Main    //Build type if building using main build def.
    }

    [SerializeField][ReadOnly] private string _buildType = "Default";
    public string BuildType => _buildType;

    public void SetBuildType(string val)
    {
        _buildType = val;
    }
}
