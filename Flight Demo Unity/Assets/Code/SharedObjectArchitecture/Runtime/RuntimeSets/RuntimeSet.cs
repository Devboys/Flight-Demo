﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects.RuntimeSets
{
    public abstract class RuntimeSet<T> : ScriptableObjectBase
    {
        //[Tooltip("The runtime version of this value. This value will be reset to the default when you enter playmode/start the game")]
        [ReadOnly] public List<T> items = new List<T>();

        //run when we first enter playmode/the application.
        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset; //make sure we dont lose values when asset is unreferenced.
            ResetSet();
        }

        public void AddItem(T itemToAdd)
        {
            //ensure no duplicates
            if (!items.Contains(itemToAdd)) items.Add(itemToAdd);
        }

        public void RemoveItem(T itemToRemove)
        {
            //ensure that items exists in list before we remove them.
            if (items.Contains(itemToRemove)) items.Remove(itemToRemove);
        }

        public void ResetSet()
        {
            items = new List<T>();
        }
    }
}
