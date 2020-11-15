using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects.RuntimeSets
{
    [CreateAssetMenu(menuName = "SharedObjects/Lists/ID Set")]
    public class IDRuntimeSet : RuntimeSet<(Vector3, bool)>
    {
        //functionality handles by SharedNumericVariable base class
    }
}
