using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects
{
    public abstract class ScriptableObjectBase : ScriptableObject
    {
#if UNITY_EDITOR
#pragma warning disable CS0414 //DO NOT USE THIS ANYWHERE ELSE (its bad practice, but its fine here because we ignore it in the build and its such as simple variable)
        //used to describe this scriptable object in the inspector. Never use this for anything.
        [SerializeField] [TextArea] private string description = " - Describe the purpose of this object here - ";
#pragma warning restore CS0414
#endif
    }
}
