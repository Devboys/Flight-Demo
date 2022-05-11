using System;
using UnityEngine;

namespace Devboys.SharedObjects.Variables
{
    [Serializable]
    public class TransformReference : SharedNumericReferenceBase<Transform, TransformVariable>
    {
        //most functionality handled by SharedNumericReferenceBase

        public Vector3 GetPosition()
        {
            return CurrentValue.position;
        }
    }
}
