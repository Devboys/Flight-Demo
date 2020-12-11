using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects.RuntimeSets {
    public class AddTransformToSet : MonoBehaviour
    {
        [SerializeField] private TransformRuntimeSet set = default;

        private void OnEnable()
        {
            set.AddItem(this.transform);
        }

        private void OnDisable()
        {
            set.RemoveItem(this.transform);
        }
    }
}