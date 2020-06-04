using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Flock/Filters/Layer")]
public class LayerFilter : ContextFilter
{
    public LayerMask mask;

    public override List<Transform> Filter (FlockAgent agent, List<Transform> orignal)
    {
        List<Transform> filtered = new List<Transform> ();

        foreach (Transform item in orignal)
        {
            if (mask == (mask | (1 << item.gameObject.layer)))
            {
                filtered.Add (item);
            }
        }
        return filtered;
    }
}