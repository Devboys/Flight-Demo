using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Devboys.SharedObjects.Variables
{
    [Serializable]
    public abstract class SharedNumericReferenceBase<T1, T2> where T2 : SharedNumericVariableBase<T1>
    {
        //simple abstraction class on top of float variable that allows you to switch between using shared SO variable and "constant" variable in the inspector.
        [SerializeField] private bool UseLocalVariable = true;

        [SerializeField] private T1 LocalVariable = default;
        [SerializeField] private T2 SharedVariable = default;

        public T1 CurrentValue
        {
            get { return UseLocalVariable ? LocalVariable : SharedVariable.CurrentValue; }
        }
    }
}