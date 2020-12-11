using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects.Variables
{
    public abstract class SharedNumericVariableBase<T> : ScriptableObjectBase
    {
        [Tooltip("The value when this script is first loaded")]
        [SerializeField] private T _defaultValue = default;
        [Tooltip("The runtime version of this value. This value will be reset to the default when you enter playmode/start the game")]
        [SerializeField] private T currentValue;

        public T DefaultValue => _defaultValue; //readonly default value

        public T CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        //run when we first enter playmode/the application.
        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset; //make sure we dont lose values when asset is unreferenced.

            ResetToDefault();
        }

        public void ResetToDefault()
        {
            currentValue = _defaultValue;
        }
    }
}
