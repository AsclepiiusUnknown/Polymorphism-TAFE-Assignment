﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    Vector2 currentVelocity = Vector2.zero;
    public float agentSmoothTime = 0.5f; //lower the faster it turns

    //Another instance of the calculate move from within Flock Behaviour which checks for agents of the same flock in its surrounding context and then moves accorningly 
    // Has very similar functionality to Cohesion behaviour but it has more control over the direction instead of just going straight until restricted by Limited radius or another opposing behaviour
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        //if no neighbours stay still
        if (filteredContext.Count == 0)
        {
            return Vector2.zero;
        }

        //Add all points together and average
        Vector2 cohesionMove = Vector2.zero;

        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }

        cohesionMove /= filteredContext.Count;
        //cohesionMove = cohesionMove / context.Count

        //Create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;

        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}
