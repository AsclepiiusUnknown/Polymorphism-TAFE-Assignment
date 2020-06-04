using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filters/Path")]
public class PathFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> orignal)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform item in orignal)
        {
            Path path = item.GetComponentInParent<Path>();
            if (path != null)
            {
                filtered.Add(item);
            }
        }

        return filtered;
    }
}
