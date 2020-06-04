﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filters/Different Flock")]
public class DifferentFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> orignal)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform item in orignal)
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();

            if (itemAgent != null && itemAgent.AgentFlock != agent.AgentFlock)
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
