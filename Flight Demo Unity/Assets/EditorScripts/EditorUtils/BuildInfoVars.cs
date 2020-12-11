using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="SHOULD_BE_INVISIBLE/DevelopmentInfoVars")]
public class BuildInfoVars : ScriptableObject
{
    [SerializeField][ReadOnly] private string _buildType = "Default";
    public string BuildType => _buildType;

    public void SetBuildType(string val)
    {
        _buildType = val;
    }
}
