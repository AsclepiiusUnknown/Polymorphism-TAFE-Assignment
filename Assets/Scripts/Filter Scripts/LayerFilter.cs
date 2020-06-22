using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filters/Layer")]
public class LayerFilter : ContextFilter
{
    public LayerMask mask;

    // A Context filter used to find all of the object/s from predetermined layer/s using a mask, and put them in a list of transforms to then be included in other equations
    public override List<Transform> Filter(FlockAgent agent, List<Transform> orignal)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform item in orignal)
        {
            if (mask == (mask | (1 << item.gameObject.layer)))
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}