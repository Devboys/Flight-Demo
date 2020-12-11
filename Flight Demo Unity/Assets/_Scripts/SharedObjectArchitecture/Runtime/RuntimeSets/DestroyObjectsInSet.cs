using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects.RuntimeSets
{
    public class DestroyObjectsInSet : MonoBehaviour
    {
        [SerializeField] private TransformRuntimeSet set = default;

        private void OnEnable()
        {
            foreach (Transform trans in set.items)
            {
                if (this.transform == trans)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void OnDisable()
        {
            set.AddItem(this.transform);
        }
    }
}