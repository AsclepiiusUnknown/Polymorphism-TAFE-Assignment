using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filters/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    // A Context filter used to find all of the agents from the same flock and put them in a list of transforms to then be included in other equations
    public override List<Transform> Filter(FlockAgent agent, List<Transform> orignal)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform item in orignal)
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();

            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
